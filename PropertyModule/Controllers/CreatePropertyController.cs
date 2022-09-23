using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PropertyModule.DAL;
using PropertyModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatePropertyController : ControllerBase
    {
        private readonly IGetDataReadersAsync _getDataReadersAsync;
        private readonly IConfiguration _configuration;
        private readonly string myConnectionString;

        public CreatePropertyController (IConfiguration configuration, IGetDataReadersAsync getDataReadersAsync)
        {
            _configuration = configuration;
            _getDataReadersAsync = getDataReadersAsync;
            myConnectionString = _configuration["ConnectionStrings:DBConnectionString"];
        }

        [Route("PropertyCreation")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<PropertyResponse> PropertyCreation([FromBody] PropertyRequest propertyRequest)
        {
            PropertyResponse listres = new PropertyResponse();
            var SavePropdata = _configuration["Queries:InsertUserPropertyData"];
            try
            {
                listres = await Task.Run(() => _getDataReadersAsync.Saveproperty<PropertyResponse, PropertyRequest>(SavePropdata, propertyRequest, myConnectionString));
                if (listres.id != null)
                {
                    listres.Responses = _configuration["Responses:PropSucess"];
                }
                else
                {
                    listres.Responses = _configuration["Responses:FailMessage"];
                }
                return listres;
            }
            catch (Exception ex)
            {
                return listres;
            }


        }

        [Route("GetPropertyDetails")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetPropertyDetails()
        {
            List<Getpropresponse> getpropresponse = new List<Getpropresponse>();
            try
            {
                var proprsql = _configuration["Queries:GetProperty"];
                getpropresponse = (List<Getpropresponse>)await Task.Run(() => _getDataReadersAsync.GetChildDataAsync<Getpropresponse, dynamic>(proprsql, myConnectionString));
                if (getpropresponse.Count() != 0)
                {
                    return Ok(getpropresponse);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Property details are not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Property details are not found.");
            }
        }




        [Route("getAllPropertiesByType")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> getAllPropertiesByType([FromBody] PropertyTypeRequest propertyTypeRequest)
        {
            List<Getpropresponse> getprop = new List<Getpropresponse>();
            try
            {
                var proprsql = _configuration["Queries:GetProperty"];
                 getprop = (List<Getpropresponse>)await Task.Run(() => _getDataReadersAsync.GetChildDataAsync<Getpropresponse, dynamic>(proprsql, propertyTypeRequest, myConnectionString));
                if (getprop.Count() != 0)
                {
                    return Ok(getprop);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Property details not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Property details not found.");
            }
        }


        [Route("getAllPropertiesByLocality")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> getAllPropertiesByLocality([FromBody] LocalityType localityType)
        {
            List<Getpropresponse> getbyloc = new List<Getpropresponse>();
            try
            {
                var losql = _configuration["Queries:GetProperty"];
                getbyloc = (List<Getpropresponse>)await Task.Run(() => _getDataReadersAsync.GetChildDataAsync<Getpropresponse, dynamic>(losql, localityType, myConnectionString));
                if (getbyloc.Count() != 0)
                {
                    return Ok(getbyloc);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Property details not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Property details not found.");
            }
        }
    }
}
