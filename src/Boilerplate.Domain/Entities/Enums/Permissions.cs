using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boilerplate.Domain.Entities.Enums;
public enum Permissions : uint
{
    CanView = 0x00000001, // 2^0
    CanAdd = 0x00000002, // 2^1
    CanModify = 0x00000004, // 2^2
    CanDelete = 0x00000008, // 2^3
    CanExecute = 0x00000010, // 2^4
    CanAudit = 0x00000020, // 2^5
    CanReview = 0x00000040, // 2^6
    CanApprove = 0x00000080, // 2^7
    Bit8 = 0x00000100, // 2^8
    Bit9 = 0x00000200, // 2^9
    Bit10 = 0x00000400, // 2^10
    Bit11 = 0x00000800, // 2^11
    Bit12 = 0x00001000, // 2^12
    Bit13 = 0x00002000, // 2^13
    Bit14 = 0x00004000, // 2^14
    Bit15 = 0x00008000, // 2^15
    Bit16 = 0x00010000, // 2^16
    Bit17 = 0x00020000, // 2^17
    Bit18 = 0x00040000, // 2^18
    Bit19 = 0x00080000, // 2^19
    Bit20 = 0x00100000, // 2^20
    Bit21 = 0x00200000, // 2^21
    Bit22 = 0x00400000, // 2^22
    Bit23 = 0x00800000, // 2^23
    Bit24 = 0x01000000, // 2^24
    Bit25 = 0x02000000, // 2^25
    Bit26 = 0x04000000, // 2^26
    Bit27 = 0x08000000, // 2^27
    Bit28 = 0x10000000, // 2^28
    Bit29 = 0x20000000, // 2^29
    Bit30 = 0x40000000, // 2^30
    Bit31 = 0x80000000  // 2^31
}
