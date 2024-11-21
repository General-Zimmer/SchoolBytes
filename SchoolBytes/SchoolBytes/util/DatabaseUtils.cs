using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SchoolBytes.util
{
    public static class DatabaseUtils
    {
        public static int SaveChangesV2(this DbContext FOK)
        {
            try
            {
                return FOK.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type ""{0}"" in state ""{1}"" 
                   has the following validation errors:",
                        eve.Entry.Entity.GetType().Name,
                        eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine(@"- Property: ""{0}"", Error: ""{1}""",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException e)
            {
                //Add your code to inspect the inner exception and/or
                //e.Entries here.
                //Or just use the debugger.
                //Added this catch (after the comments below) to make it more obvious 
                //how this code might help this specific problem
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            Debug.Print("WTF MAAAAAN");
            return 0;
        }
    }
}