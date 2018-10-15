using System;
using System.Collections.Generic;
using System.Linq;
using DarcEuphoria.Euphoric.Enums;

namespace DarcEuphoria.Euphoric
{
    public class Allocator
    {
        public Dictionary<IntPtr, IntPtr> AllocatedSize = new Dictionary<IntPtr, IntPtr>();

        public IntPtr AlloacNewPage(IntPtr size)
        {
            var Address = WinApi.VirtualAllocEx(Memory.PHandle, IntPtr.Zero, (IntPtr) 4096,
                (int) FreeType.MEM_COMMIT | (int) FreeType.MEM_RESERVE, 0x40);

            AllocatedSize.Add(Address, size);

            return Address;
        }

        public void Free()
        {
            foreach (var key in AllocatedSize)
                WinApi.VirtualFreeEx(Memory.PHandle, key.Key, 4096,
                    (int) FreeType.MEM_COMMIT | (int) FreeType.MEM_RESERVE);
        }

        public IntPtr Alloc(int size)
        {
            for (var i = 0; i < AllocatedSize.Count; ++i)
            {
                var key = AllocatedSize.ElementAt(i).Key;
                var value = (int) AllocatedSize[key] + size;
                if (value < 4096)
                {
                    var CurrentAddres = IntPtr.Add(key, (int) AllocatedSize[key]);
                    AllocatedSize[key] = new IntPtr(value);
                    return CurrentAddres;
                }
            }

            return AlloacNewPage(new IntPtr(size));
        }
    }
}