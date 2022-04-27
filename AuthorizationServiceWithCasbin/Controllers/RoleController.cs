using Data.Model.PaginationModel;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Core;
using System;
using System.Threading.Tasks;
using static Data.Enums.EntityEnum;

namespace AuthorizationServiceWithCasbin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles([FromQuery] PagingParam<RoleSortCriteria> paginationModel, [FromQuery] RoleSearchModel searchModel)
        {
            var result = await _roleService.GetRoles(paginationModel, searchModel);
            if (result.IsSuccess) return Ok(result.ResponseSuccess);
            return BadRequest(result.ResponseFailed);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            ResultModel result;

            result = await _roleService.GetRoleById(id);
            if (result.IsSuccess) return Ok(result.ResponseSuccess);
            return NotFound(result.ResponseFailed);
        }

        [HttpPost()]
        public async Task<IActionResult> Add(RoleCreateModel model)
        {
            var result = await _roleService.AddRole(model);
            if (result.IsSuccess) return Ok(result.ResponseSuccess);
            return BadRequest(result.ResponseFailed);
        }
        [HttpPut()]
        public async Task<IActionResult> UpdateRole(RoleUpdateModel model)
        {
            var result = await _roleService.UpdateRole(model);
            if (result.IsSuccess) return Ok(result.ResponseSuccess);
            return BadRequest(result.ResponseFailed);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = RolesConstants.ADMIN)]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var result = await _roleService.DeleteRole(id);
            if (result.IsSuccess)
                return NoContent();
            else return BadRequest(result.ResponseFailed);
        }


    }
}
