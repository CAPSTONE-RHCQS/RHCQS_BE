using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RHCQS_BE.Extenstion;
using RHCQS_DataAccessObjects.Models;
using RHCQS_Services.Interface;
using System.Collections.Generic;

namespace RHCQS_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Retrieves the list of all roles.
        /// </summary>
        /// <returns>List of roles in the system</returns>
        [HttpGet(ApiEndPointConstant.Role.RoleEndpoint)]
        [ProducesResponseType(typeof(IEnumerable<Role>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Role>>> GetListRoleAsync()
        {
            var roles = await _roleService.GetAllRolesAsync();
            var response = JsonConvert.SerializeObject(roles, Formatting.Indented);
            return Ok(response);
        }

        [HttpGet(ApiEndPointConstant.Role.TotalRoleEndpoint)]
        public async Task<ActionResult<int>> GetTotalAccountCount()
        {
            var totalRoleCount = await _roleService.GetTotalRoleCountAsync();
            return Ok(totalRoleCount);
        }
    }
}
