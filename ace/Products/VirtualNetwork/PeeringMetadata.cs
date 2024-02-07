namespace ACE.Products.VirtualNetwork;

internal sealed class PeeringMetadata
{
    public PeeringMetadata(PeeringType type)
    {
        this.Type = type;
        VNetLocation = "N/A";
        RemoteVNetLocation = "N/A";
    }

    public PeeringMetadata(PeeringType type, string vnetLocation, string remoteVNetLocation)
    {
        this.Type = type;
        this.VNetLocation = vnetLocation;
        this.RemoteVNetLocation = remoteVNetLocation;
    }

    public PeeringType Type { get; }
    public string VNetLocation { get; }
    public string RemoteVNetLocation { get; }
}