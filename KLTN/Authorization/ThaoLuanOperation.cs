using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Authorization
{
    public class ThaoLuanOperation
    {
        public static readonly OperationAuthorizationRequirement Create = new OperationAuthorizationRequirement { Name = "Create" };
        public static readonly OperationAuthorizationRequirement Read = new OperationAuthorizationRequirement { Name = "Read" };
        public static readonly OperationAuthorizationRequirement Update = new OperationAuthorizationRequirement { Name = "Update" };
        public static readonly OperationAuthorizationRequirement Delete = new OperationAuthorizationRequirement { Name = "Delete" };

    }

    
}
