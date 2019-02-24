namespace CleaseSolution
{
    using System.Collections.Generic;

    public interface IPleaseClient
    {
        List<Hardware> GetHardwareList();

        List<Hardware> GetHardwareList(string platform);

        List<Hardware> GetLeasedHardwareList();

        bool AddHardware(Hardware hardware);

        bool LeaseHardware(Platform platform, int duration);
    }
}