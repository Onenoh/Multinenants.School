﻿using Application.Features.Identity.Roles;
using Application.Features.Identity.Roles.Commands;
using Application.Features.Identity.Roles.Queries;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(SchoolAction.Create, SchoolFeature.Roles)]
        public async Task<IActionResult> AddRoleAsync([FromBody] CreateRoleRequest createRole)
        {
            var response = await Sender.Send(new CreateRoleCommand { RoleRequest = createRole });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Roles)]
        public async Task<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleRequest updateRole)
        {
            var response = await Sender.Send(new UpdateRoleCommand { UpdateRole = updateRole });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update-permission")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.RoleClaims)]
        public async Task<IActionResult> UpdateRoleClaimsAsync([FromBody] UpdateRolePermissionsRequest updateRoleClaims)
        {
            var response = await Sender.Send(new UpdateRolePermissionsCommand { UpdateRolePermissions = updateRoleClaims });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("delete/{roleId}")]
        [ShouldHavePermission(SchoolAction.Delete, SchoolFeature.Roles)]
        public async Task<IActionResult> DeleteRoleAsync(string roleId)
        {
            var response = await Sender.Send(new DeleteRoleCommand { RoleId = roleId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("all")]
        [ShouldHavePermission(SchoolAction.View, SchoolFeature.Roles)]
        public async Task<IActionResult> GetRoleAsync()
        {
            var response = await Sender.Send(new GetRoleQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("partial/{roleId}")]
        [ShouldHavePermission(SchoolAction.View, SchoolFeature.Roles)]
        public async Task<IActionResult> GetPartialRoleByIdAsync(string roleId)
        {
            var response = await Sender.Send(new GetRoleByIdQuery { RoleId = roleId});
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("full/{roleId}")]
        [ShouldHavePermission(SchoolAction.View, SchoolFeature.Roles)]
        public async Task<IActionResult> GetDetailedRoleByIdAsync(string roleId)
        {
            var response = await Sender.Send(new GetRoleWithPermissionQuery { RoleId = roleId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}