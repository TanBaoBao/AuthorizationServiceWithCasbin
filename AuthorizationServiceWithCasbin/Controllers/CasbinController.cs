using Data.Models;
using Data.Requests.Casbin;
using Microsoft.AspNetCore.Mvc;
using Service.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorizationServiceWithCasbin.WebApi.Controllers
{
    [Route("api/casbin")]
    [ApiController]
    public class CasbinController : ControllerBase
    {
        private readonly ICasbinService _casbinService;

        public CasbinController(ICasbinService casbinService)
        {
            _casbinService = casbinService;
        }

        /// <summary>
        /// Lấy tất cả các subjects
        /// </summary>
        /// <returns></returns>
        [HttpGet("subjects")]
        public IActionResult GetAllSubjects()
        {
            var result = new ResultModel();
            result.ResponseSuccess = _casbinService.GetAllSubjects();
            return Ok(result);
        }

        /// <summary>
        /// Lấy tất cả các actions
        /// </summary>
        /// <returns></returns>
        [HttpGet("actions")]
        public IActionResult GetAllActions()
        {
            var result = new ResultModel();
            result.ResponseSuccess = _casbinService.GetAllActions();
            return Ok(result);
        }

        /// <summary>
        /// Lấy tất cả các objects
        /// </summary>
        /// <returns></returns>
        [HttpGet("objects")]
        public IActionResult GetAllObjects()
        {
            var result = new ResultModel();
            result.ResponseSuccess = _casbinService.GetAllObjects();
            return Ok(result);
        }

        /// <summary>
        /// Lấy quyền hành hiện tại
        /// </summary>
        /// <returns></returns>
        [HttpGet("policies")]
        public IActionResult GetPolicy(string role, string domain)
        {
            var result = new ResultModel();
            result.ResponseSuccess = _casbinService.GetPolicy(role, domain);
            return Ok(result);
        }

        /// <summary>
        /// Thêm quyền hành
        /// </summary>
        /// <param name="addPolicyRequest"></param>
        /// <returns></returns>
        [HttpPost("policies")]
        public async Task<IActionResult> AddPolicies(List<AddPolicyRequest> addPoliciesRequest)
        {
            var result = new ResultModel();
            try
            {
                var rs = await _casbinService.AddPolicies(addPoliciesRequest);
                result.ResponseSuccess = rs.ToString();
                return Ok(result);
            }
            catch (Exception e)
            {
                result.ResponseFailed = e.Message;
                return BadRequest(result);
            }
        }

        [HttpPost("rolesforuser")]
        public async Task<IActionResult> AddRoleForUser(AddRolesForUserRequest addRoleForUserRequest)
        {
            var result = new ResultModel();
            try
            {
                await _casbinService.AddRolesForUser(addRoleForUserRequest);
                result.ResponseSuccess = "Thêm role thành công";
                return Ok(result);
            }
            catch (Exception e)
            {
                result.ResponseFailed = e.Message;
                return BadRequest(result);
            }
        }
        [HttpDelete("roleforuser")]
        public async Task<IActionResult> DeleteRoleForUser(RemoveRoleForUserRequest removeRoleForUserRequest)
        {
            var result = new ResultModel();
            try
            {
                await _casbinService.RemoveRoleForUser(removeRoleForUserRequest);
                result.ResponseSuccess = "Xóa role thành công";
                return Ok(result);
            }
            catch (Exception e)
            {
                result.ResponseFailed = e.Message;
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Xóa quyền hành
        /// </summary>
        /// <param name="removePolicyRequest"></param>
        /// <returns></returns>
        [HttpDelete("policy")]
        public async Task<IActionResult> RemovePolicy(RemovePolicyRequest removePolicyRequest)
        {
            var result = new ResultModel();
            try
            {
                await _casbinService.RemovePolicy(removePolicyRequest);
                result.ResponseSuccess = "Thêm quyền hành thành công";
                return Ok(result);
            }
            catch (Exception e)
            {
                result.ResponseFailed = e.Message;
                return Ok(result);
            }
        }
    }
}