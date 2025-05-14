namespace WF.Mimetic.Application.DTO.Nodes.Printers;

using System;

public class PrinterCreateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Script { get; set; }
}
