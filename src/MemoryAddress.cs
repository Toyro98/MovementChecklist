namespace MovementChecklist
{
    public class MemoryAddress
    {
        public uint target;
        public uint baseAddress;
        public uint pointer;
        public int[] offsets;

        public MemoryAddress(int offset)
        {
            switch (GameVersion.Current)
            {
                case Version.Steam:
                case Version.GoG:
                    pointer = 0x01C553D0;
                    offsets = new int[] { 0xCC, 0x1CC, 0x2F8, offset };
                    break;

                case Version.Origin:
                case Version.Dvd:
                case Version.Reloaded:
                    pointer = 0x01C6E50C;
                    offsets = new int[] { 0xCC, 0x1CC, 0x2F8, offset };
                    break;

                case Version.OriginDLC:
                    pointer = 0x01C7561C;
                    offsets = new int[] { 0xE4, 0x1CC, 0x2F8, offset };
                    break;

                case Version.OriginAsia:
                    pointer = 0x01B8BE54;
                    offsets = new int[] { 0xCC, 0x1CC, 0x2F8, offset };
                    break;
            }
        }

        public void Update(MemoryHelper memoryHelper)
        {
            uint newBaseAddress = memoryHelper.GetBaseAddress(pointer);

            if (newBaseAddress != 0)
            {
                baseAddress = newBaseAddress;
            }

            target = MemoryUtils.OffsetCalculator(memoryHelper, baseAddress, offsets);
        }
    }
}