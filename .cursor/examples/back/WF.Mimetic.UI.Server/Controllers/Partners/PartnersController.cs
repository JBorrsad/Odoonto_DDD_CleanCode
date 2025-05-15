namespace WF.Mimetic.UI.Server.Controllers.Partners;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WF.Mimetic.Application.Sidecar.DTO.Partners.Partners;
using WF.Mimetic.Application.Sidecar.Interfaces.Partners;

[Route("api/v0/sidecar/[controller]")]
[ApiController]
public class PartnersController : ControllerBase
{
    private readonly IPartnerAppService _partnerAppService;

    public PartnersController(IPartnerAppService partnerAppService)
    {
        _partnerAppService = partnerAppService;
    }

    [HttpGet(Name = "PartnersGetAll")]
    public Task<IEnumerable<PartnerReadDto>> GetAll()
    {
        return _partnerAppService.GetAll();
    }

    [HttpGet("{id}", Name = "PartnersGetById")]
    public Task<PartnerReadDto> GetById(Guid id)
    {
        return _partnerAppService.GetById(id);
    }

    [HttpPut("{id}", Name = "PartnersEdit")]
    public Task Edit(Guid id, PartnerEditDto data)
    {
        return _partnerAppService.Edit(id, data);
    }

    [HttpDelete("{id}", Name = "PartnersRemove")]
    public Task Remove(Guid id)
    {
        return _partnerAppService.Remove(id);
    }
}
