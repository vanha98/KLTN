using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KLTN.Authorization.Handlers
{
    public class OwnerThaoLuanAuthorization : AuthorizationHandler<OperationAuthorizationRequirement, BaiPost>
    {

        private readonly IIdentity _identityService;

        public OwnerThaoLuanAuthorization(IIdentity identityService)
        {
            _identityService = identityService;
        }

        protected override Task HandleRequirementAsync(
               AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, BaiPost resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName)
            {
                return Task.CompletedTask;
            }

            var user = _identityService.Get(context.User);//.Get(context.User);
            if (user == resource.IdnguoiTao)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

    }
}
