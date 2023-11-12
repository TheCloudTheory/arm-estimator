internal class VirtualMachineStorageProfile
{
    public VirtualMachineImageReference? imageReference { get; set; }
}

internal class VirtualMachineImageReference
{
    public string? offer { get; set; }
    public string? publisher { get; set; }
    public string? sku { get; set; }
    public string? version { get; set; }
}