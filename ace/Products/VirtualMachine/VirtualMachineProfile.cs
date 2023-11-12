internal class VirtualMachineProfile
{
    public VirtualMachineHardwareProfile? hardwareProfile { get; set; }
    public VirtualMachineStorageProfile? storageProfile { get; set; }
    public string? priority { get; set; }
}
