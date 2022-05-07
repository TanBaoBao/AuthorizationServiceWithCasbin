using Casbin.Adapter.EFCore;
using Data.Requests.Casbin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetCasbin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Core
{
    public interface ICasbinService
    {
        bool Enforce(string sub, string dom, string obj, string act);
        Task<bool> AddPolicies(List<AddPolicyRequest> addPoliciesRequest);
        Task RemovePolicy(RemovePolicyRequest removePolicyRequest);
        Task AddRolesForUser(AddRolesForUserRequest request);
        Task RemoveRoleForUser(RemoveRoleForUserRequest removeRoleForUserRequest);
        List<string> GetAllObjects();
        List<string> GetAllActions();
        List<string> GetAllSubjects();
        List<List<string>> GetPolicy(string role, string domain);
    }

    public class CasbinService : ICasbinService
    {
        private readonly Enforcer _enforcer;
        public CasbinService(IConfiguration configuration)
        {
            var options = new DbContextOptionsBuilder<CasbinDbContext<int>>()
                .UseNpgsql(configuration["AppDatabaseSettings:ConnectionString"])
                .Options;
            var dbContext = new CasbinDbContext<int>(options);
            dbContext.Database.EnsureCreated();
            var efCoreAdapter = new EFCoreAdapter<int>(dbContext);
            _enforcer = new Enforcer("Resources/Casbin/rbac_model.conf", efCoreAdapter);
            //_enforcer = new Enforcer("Resources/Casbin/rbac_model.conf", "Resources/Casbin/rbac_with_domain_pattern_policy.csv");
        }

        public bool Enforce(string sub, string dom, string obj, string act)
        {
            return _enforcer.Enforce(sub, dom, obj, act);
        }

        public List<string> GetAllSubjects()
        {
            return _enforcer.GetAllSubjects();
        }
        
        public List<string> GetAllActions()
        {
            return _enforcer.GetAllActions();
        }
        
        public List<string> GetAllObjects()
        {
            return _enforcer.GetAllObjects();
        }

        public List<List<string>> GetPolicy(string role, string domain)
        {
            return _enforcer.GetPermissionsForUserInDomain(role, domain);
        }
        public async Task AddRolesForUser(AddRolesForUserRequest request)
        {
            await _enforcer.AddRolesForUserAsync(request.UserId, request.Roles, request.Domain);
        }

        public async Task RemoveRoleForUser(RemoveRoleForUserRequest request)
        {
            await _enforcer.DeleteRoleForUserAsync(request.UserId, request.Role, request.Domain);
        }
        public async Task<bool> AddPolicies(List<AddPolicyRequest> addPoliciesRequest)
        {
            List<List<string>> list = new List<List<string>>();
            foreach (var policy in addPoliciesRequest)
            {
                list.Add(new List<string> { policy.Subject, policy.Domain, policy.Object, policy.Action });
            }
            
            return await _enforcer.AddPoliciesAsync(list);
        }

        public async Task RemovePolicy(RemovePolicyRequest removePolicyRequest)
        {
            await _enforcer.RemovePolicyAsync(removePolicyRequest.Subject, removePolicyRequest.Domain, removePolicyRequest.Object, removePolicyRequest.Action);
        }
    }
}