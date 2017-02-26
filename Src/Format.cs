namespace VulkanCore
{
    /// <summary>
    /// Available image formats.
    /// </summary>
    public enum Format
    {
        /// <summary>
        /// The format is not specified.
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// A two-component, 8-bit packed unsigned normalized format that has a 4-bit R component in
        /// bits 4..7, and a 4-bit G component in bits 0..3.
        /// </summary>
        R4G4UNormPack8 = 1,
        /// <summary>
        /// A four-component, 16-bit packed unsigned normalized format that has a 4-bit R component
        /// in bits 12..15, a 4-bit G component in bits 8..11, a 4-bit B component in bits 4..7, and
        /// a 4-bit A component in bits 0..3.
        /// </summary>
        R4G4B4A4UNormPack16 = 2,
        /// <summary>
        /// A four-component, 16-bit packed unsigned normalized format that has a 4-bit B component
        /// in bits 12..15, a 4-bit G component in bits 8..11, a 4-bit R component in bits 4..7, and
        /// a 4-bit A component in bits 0..3.
        /// </summary>
        B4G4R4A4UNormPack16 = 3,
        /// <summary>
        /// A three-component, 16-bit packed unsigned normalized format that has a 5-bit R component
        /// in bits 11..15, a 6-bit G component in bits 5..10, and a 5-bit B component in bits 0..4.
        /// </summary>
        R5G6B5UNormPack16 = 4,
        /// <summary>
        /// A three-component, 16-bit packed unsigned normalized format that has a 5-bit B component
        /// in bits 11..15, a 6-bit G component in bits 5..10, and a 5-bit R component in bits 0..4.
        /// </summary>
        B5G6R5UNormPack16 = 5,
        /// <summary>
        /// A four-component, 16-bit packed unsigned normalized format that has a 5-bit R component
        /// in bits 11..15, a 5-bit G component in bits 6..10, a 5-bit B component in bits 1..5, and
        /// a 1-bit A component in bit 0.
        /// </summary>
        R5G5B5A1UNormPack16 = 6,
        /// <summary>
        /// A four-component, 16-bit packed unsigned normalized format that has a 5-bit B component
        /// in bits 11..15, a 5-bit G component in bits 6..10, a 5-bit R component in bits 1..5, and
        /// a 1-bit A component in bit 0.
        /// </summary>
        B5G5R5A1UNormPack16 = 7,
        /// <summary>
        /// A four-component, 16-bit packed unsigned normalized format that has a 1-bit A component
        /// in bit 15, a 5-bit R component in bits 10..14, a 5-bit G component in bits 5..9, and a
        /// 5-bit B component in bits 0..4.
        /// </summary>
        A1R5G5B5UNormPack16 = 8,
        /// <summary>
        /// A one-component, 8-bit unsigned normalized format that has a single 8-bit R component.
        /// </summary>
        R8UNorm = 9,
        /// <summary>
        /// A one-component, 8-bit signed normalized format that has a single 8-bit R component.
        /// </summary>
        R8SNorm = 10,
        /// <summary>
        /// A one-component, 8-bit unsigned scaled integer format that has a single 8-bit R component.
        /// </summary>
        R8UScaled = 11,
        /// <summary>
        /// A one-component, 8-bit signed scaled integer format that has a single 8-bit R component.
        /// </summary>
        R8SScaled = 12,
        /// <summary>
        /// A one-component, 8-bit unsigned integer format that has a single 8-bit R component.
        /// </summary>
        R8UInt = 13,
        /// <summary>
        /// A one-component, 8-bit signed integer format that has a single 8-bit R component.
        /// </summary>
        R8SInt = 14,
        /// <summary>
        /// A one-component, 8-bit unsigned normalized format that has a single 8-bit R component
        /// stored with sRGB nonlinear encoding.
        /// </summary>
        R8SRgb = 15,
        /// <summary>
        /// A two-component, 16-bit unsigned normalized format that has an 8-bit R component in byte
        /// 0, and an 8-bit G component in byte 1.
        /// </summary>
        R8G8UNorm = 16,
        /// <summary>
        /// A two-component, 16-bit signed normalized format that has an 8-bit R component in byte 0,
        /// and an 8-bit G component in byte 1.
        /// </summary>
        R8G8SNorm = 17,
        /// <summary>
        /// A two-component, 16-bit unsigned scaled integer format that has an 8-bit R component in
        /// byte 0, and an 8-bit G component in byte 1.
        /// </summary>
        R8G8UScaled = 18,
        /// <summary>
        /// A two-component, 16-bit signed scaled integer format that has an 8-bit R component in
        /// byte 0, and an 8-bit G component in byte 1.
        /// </summary>
        R8G8SScaled = 19,
        /// <summary>
        /// A two-component, 16-bit unsigned integer format that has an 8-bit R component in byte 0,
        /// and an 8-bit G component in byte 1.
        /// </summary>
        R8G8UInt = 20,
        /// <summary>
        /// A two-component, 16-bit signed integer format that has an 8-bit R component in byte 0,
        /// and an 8-bit G component in byte 1.
        /// </summary>
        R8G8SInt = 21,
        /// <summary>
        /// A two-component, 16-bit unsigned normalized format that has an 8-bit R component stored
        /// with sRGB nonlinear encoding in byte 0, and an 8-bit G component stored with sRGB
        /// nonlinear encoding in byte 1.
        /// </summary>
        R8G8SRgb = 22,
        /// <summary>
        /// A three-component, 24-bit unsigned normalized format that has an 8-bit R component in
        /// byte 0, an 8-bit G component in byte 1, and an 8-bit B component in byte 2.
        /// </summary>
        R8G8B8UNorm = 23,
        /// <summary>
        /// A three-component, 24-bit signed normalized format that has an 8-bit R component in byte
        /// 0, an 8-bit G component in byte 1, and an 8-bit B component in byte 2.
        /// </summary>
        R8G8B8SNorm = 24,
        /// <summary>
        /// A three-component, 24-bit unsigned scaled format that has an 8-bit R component in byte 0,
        /// an 8-bit G component in byte 1, and an 8-bit B component in byte 2.
        /// </summary>
        R8G8B8UScaled = 25,
        /// <summary>
        /// A three-component, 24-bit signed scaled format that has an 8-bit R component in byte 0,
        /// an 8-bit G component in byte 1, and an 8-bit B component in byte 2.
        /// </summary>
        R8G8B8SScaled = 26,
        /// <summary>
        /// A three-component, 24-bit unsigned integer format that has an 8-bit R component in byte
        /// 0, an 8-bit G component in byte 1, and an 8-bit B component in byte 2.
        /// </summary>
        R8G8B8UInt = 27,
        /// <summary>
        /// A three-component, 24-bit signed integer format that has an 8-bit R component in byte 0,
        /// an 8-bit G component in byte 1, and an 8-bit B component in byte 2.
        /// </summary>
        R8G8B8SInt = 28,
        /// <summary>
        /// A three-component, 24-bit unsigned normalized format that has an 8-bit R component stored
        /// with sRGB nonlinear encoding in byte 0, an 8-bit G component stored with sRGB nonlinear
        /// encoding in byte 1, and an 8-bit B component stored with sRGB nonlinear encoding in byte 2.
        /// </summary>
        R8G8B8SRgb = 29,
        /// <summary>
        /// A three-component, 24-bit unsigned normalized format that has an 8-bit B component in
        /// byte 0, an 8-bit G component in byte 1, and an 8-bit R component in byte 2.
        /// </summary>
        B8G8R8UNorm = 30,
        /// <summary>
        /// A three-component, 24-bit signed normalized format that has an 8-bit B component in byte
        /// 0, an 8-bit G component in byte 1, and an 8-bit R component in byte 2.
        /// </summary>
        B8G8R8SNorm = 31,
        /// <summary>
        /// A three-component, 24-bit unsigned scaled format that has an 8-bit B component in byte 0,
        /// an 8-bit G component in byte 1, and an 8-bit R component in byte 2.
        /// </summary>
        B8G8R8UScaled = 32,
        /// <summary>
        /// A three-component, 24-bit signed scaled format that has an 8-bit B component in byte 0,
        /// an 8-bit G component in byte 1, and an 8-bit R component in byte 2.
        /// </summary>
        B8G8R8SScaled = 33,
        /// <summary>
        /// A three-component, 24-bit unsigned integer format that has an 8-bit B component in byte
        /// 0, an 8-bit G component in byte 1, and an 8-bit R component in byte 2.
        /// </summary>
        B8G8R8UInt = 34,
        /// <summary>
        /// A three-component, 24-bit signed integer format that has an 8-bit B component in byte 0,
        /// an 8-bit G component in byte 1, and an 8-bit R component in byte 2.
        /// </summary>
        B8G8R8SInt = 35,
        /// <summary>
        /// A three-component, 24-bit unsigned normalized format that has an 8-bit B component stored
        /// with sRGB nonlinear encoding in byte 0, an 8-bit G component stored with sRGB nonlinear
        /// encoding in byte 1, and an 8-bit R component stored with sRGB nonlinear encoding in byte 2.
        /// </summary>
        B8G8R8SRgb = 36,
        /// <summary>
        /// A four-component, 32-bit unsigned normalized format that has an 8-bit R component in byte
        /// 0, an 8-bit G component in byte 1, an 8-bit B component in byte 2, and an 8-bit A
        /// component in byte 3.
        /// </summary>
        R8G8B8A8UNorm = 37,
        /// <summary>
        /// A four-component, 32-bit signed normalized format that has an 8-bit R component in byte
        /// 0, an 8-bit G component in byte 1, an 8-bit B component in byte 2, and an 8-bit A
        /// component in byte 3.
        /// </summary>
        R8G8B8A8SNorm = 38,
        /// <summary>
        /// A four-component, 32-bit unsigned scaled format that has an 8-bit R component in byte 0,
        /// an 8-bit G component in byte 1, an 8-bit B component in byte 2, and an 8-bit A component
        /// in byte 3.
        /// </summary>
        R8G8B8A8UScaled = 39,
        /// <summary>
        /// A four-component, 32-bit signed scaled format that has an 8-bit R component in byte 0, an
        /// 8-bit G component in byte 1, an 8-bit B component in byte 2, and an 8-bit A component in
        /// byte 3.
        /// </summary>
        R8G8B8A8SScaled = 40,
        /// <summary>
        /// A four-component, 32-bit unsigned integer format that has an 8-bit R component in byte 0,
        /// an 8-bit G component in byte 1, an 8-bit B component in byte 2, and an 8-bit A component
        /// in byte 3.
        /// </summary>
        R8G8B8A8UInt = 41,
        /// <summary>
        /// A four-component, 32-bit signed integer format that has an 8-bit R component in byte 0,
        /// an 8-bit G component in byte 1, an 8-bit B component in byte 2, and an 8-bit A component
        /// in byte 3.
        /// </summary>
        R8G8B8A8SInt = 42,
        /// <summary>
        /// A four-component, 32-bit unsigned normalized format that has an 8-bit R component stored
        /// with sRGB nonlinear encoding in byte 0, an 8-bit G component stored with sRGB nonlinear
        /// encoding in byte 1, an 8-bit B component stored with sRGB nonlinear encoding in byte 2,
        /// and an 8-bit A component in byte 3.
        /// </summary>
        R8G8B8A8SRgb = 43,
        /// <summary>
        /// A four-component, 32-bit unsigned normalized format that has an 8-bit B component in byte
        /// 0, an 8-bit G component in byte 1, an 8-bit R component in byte 2, and an 8-bit A
        /// component in byte 3.
        /// </summary>
        B8G8R8A8UNorm = 44,
        /// <summary>
        /// A four-component, 32-bit signed normalized format that has an 8-bit B component in byte
        /// 0, an 8-bit G component in byte 1, an 8-bit R component in byte 2, and an 8-bit A
        /// component in byte 3.
        /// </summary>
        B8G8R8A8SNorm = 45,
        /// <summary>
        /// A four-component, 32-bit unsigned scaled format that has an 8-bit B component in byte 0,
        /// an 8-bit G component in byte 1, an 8-bit R component in byte 2, and an 8-bit A component
        /// in byte 3.
        /// </summary>
        B8G8R8A8UScaled = 46,
        /// <summary>
        /// A four-component, 32-bit signed scaled format that has an 8-bit B component in byte 0, an
        /// 8-bit G component in byte 1, an 8-bit R component in byte 2, and an 8-bit A component in
        /// byte 3.
        /// </summary>
        B8G8R8A8SScaled = 47,
        /// <summary>
        /// A four-component, 32-bit unsigned integer format that has an 8-bit B component in byte 0,
        /// an 8-bit G component in byte 1, an 8-bit R component in byte 2, and an 8-bit A component
        /// in byte 3.
        /// </summary>
        B8G8R8A8UInt = 48,
        /// <summary>
        /// A four-component, 32-bit signed integer format that has an 8-bit B component in byte 0,
        /// an 8-bit G component in byte 1, an 8-bit R component in byte 2, and an 8-bit A component
        /// in byte 3.
        /// </summary>
        B8G8R8A8SInt = 49,
        /// <summary>
        /// A four-component, 32-bit unsigned normalized format that has an 8-bit B component stored
        /// with sRGB nonlinear encoding in byte 0, an 8-bit G component stored with sRGB nonlinear
        /// encoding in byte 1, an 8-bit R component stored with sRGB nonlinear encoding in byte 2,
        /// and an 8-bit A component in byte 3.
        /// </summary>
        B8G8R8A8SRgb = 50,
        /// <summary>
        /// A four-component, 32-bit packed unsigned normalized format that has an 8-bit A component
        /// in bits 24..31, an 8-bit B component in bits 16..23, an 8-bit G component in bits 8..15,
        /// and an 8-bit R component in bits 0..7.
        /// </summary>
        A8B8G8R8UNormPack32 = 51,
        /// <summary>
        /// A four-component, 32-bit packed signed normalized format that has an 8-bit A component in
        /// bits 24..31, an 8-bit B component in bits 16..23, an 8-bit G component in bits 8..15, and
        /// an 8-bit R component in bits 0..7.
        /// </summary>
        A8B8G8R8SNormPack32 = 52,
        /// <summary>
        /// A four-component, 32-bit packed unsigned scaled integer format that has an 8-bit A
        /// component in bits 24..31, an 8-bit B component in bits 16..23, an 8-bit G component in
        /// bits 8..15, and an 8-bit R component in bits 0..7.
        /// </summary>
        A8B8G8R8UScaledPack32 = 53,
        /// <summary>
        /// A four-component, 32-bit packed signed scaled integer format that has an 8-bit A
        /// component in bits 24..31, an 8-bit B component in bits 16..23, an 8-bit G component in
        /// bits 8..15, and an 8-bit R component in bits 0..7.
        /// </summary>
        A8B8G8R8SScaledPack32 = 54,
        /// <summary>
        /// A four-component, 32-bit packed unsigned integer format that has an 8-bit A component in
        /// bits 24..31, an 8-bit B component in bits 16..23, an 8-bit G component in bits 8..15, and
        /// an 8-bit R component in bits 0..7.
        /// </summary>
        A8B8G8R8UIntPack32 = 55,
        /// <summary>
        /// A four-component, 32-bit packed signed integer format that has an 8-bit A component in
        /// bits 24..31, an 8-bit B component in bits 16..23, an 8-bit G component in bits 8..15, and
        /// an 8-bit R component in bits 0..7.
        /// </summary>
        A8B8G8R8SIntPack32 = 56,
        /// <summary>
        /// A four-component, 32-bit packed unsigned normalized format that has an 8-bit A component
        /// in bits 24..31, an 8-bit B component stored with sRGB nonlinear encoding in bits 16..23,
        /// an 8-bit G component stored with sRGB nonlinear encoding in bits 8..15, and an 8-bit R
        /// component stored with sRGB nonlinear encoding in bits 0..7.
        /// </summary>
        A8B8G8R8SRgbPack32 = 57,
        /// <summary>
        /// A four-component, 32-bit packed unsigned normalized format that has a 2-bit A component
        /// in bits 30..31, a 10-bit R component in bits 20..29, a 10-bit G component in bits 10..19,
        /// and a 10-bit B component in bits 0..9.
        /// </summary>
        A2R10G10B10UNormPack32 = 58,
        /// <summary>
        /// A four-component, 32-bit packed signed normalized format that has a 2-bit A component in
        /// bits 30..31, a 10-bit R component in bits 20..29, a 10-bit G component in bits 10..19,
        /// and a 10-bit B component in bits 0..9.
        /// </summary>
        A2R10G10B10SNormPack32 = 59,
        /// <summary>
        /// A four-component, 32-bit packed unsigned scaled integer format that has a 2-bit A
        /// component in bits 30..31, a 10-bit R component in bits 20..29, a 10-bit G component in
        /// bits 10..19, and a 10-bit B component in bits 0..9.
        /// </summary>
        A2R10G10B10UScaledPack32 = 60,
        /// <summary>
        /// A four-component, 32-bit packed signed scaled integer format that has a 2-bit A component
        /// in bits 30..31, a 10-bit R component in bits 20..29, a 10-bit G component in bits 10..19,
        /// and a 10-bit B component in bits 0..9.
        /// </summary>
        A2R10G10B10SScaledPack32 = 61,
        /// <summary>
        /// A four-component, 32-bit packed unsigned integer format that has a 2-bit A component in
        /// bits 30..31, a 10-bit R component in bits 20..29, a 10-bit G component in bits 10..19,
        /// and a 10-bit B component in bits 0..9.
        /// </summary>
        A2R10G10B10UIntPack32 = 62,
        /// <summary>
        /// A four-component, 32-bit packed signed integer format that has a 2-bit A component in
        /// bits 30..31, a 10-bit R component in bits 20..29, a 10-bit G component in bits 10..19,
        /// and a 10-bit B component in bits 0..9.
        /// </summary>
        A2R10G10B10SIntPack32 = 63,
        /// <summary>
        /// A four-component, 32-bit packed unsigned normalized format that has a 2-bit A component
        /// in bits 30..31, a 10-bit B component in bits 20..29, a 10-bit G component in bits 10..19,
        /// and a 10-bit R component in bits 0..9.
        /// </summary>
        A2B10G10R10UNormPack32 = 64,
        /// <summary>
        /// A four-component, 32-bit packed signed normalized format that has a 2-bit A component in
        /// bits 30..31, a 10-bit B component in bits 20..29, a 10-bit G component in bits 10..19,
        /// and a 10-bit R component in bits 0..9.
        /// </summary>
        A2B10G10R10SNormPack32 = 65,
        /// <summary>
        /// A four-component, 32-bit packed unsigned scaled integer format that has a 2-bit A
        /// component in bits 30..31, a 10-bit B component in bits 20..29, a 10-bit G component in
        /// bits 10..19, and a 10-bit R component in bits 0..9.
        /// </summary>
        A2B10G10R10UScaledPack32 = 66,
        /// <summary>
        /// A four-component, 32-bit packed signed scaled integer format that has a 2-bit A component
        /// in bits 30..31, a 10-bit B component in bits 20..29, a 10-bit G component in bits 10..19,
        /// and a 10-bit R component in bits 0..9.
        /// </summary>
        A2B10G10R10SScaledPack32 = 67,
        /// <summary>
        /// A four-component, 32-bit packed unsigned integer format that has a 2-bit A component in
        /// bits 30..31, a 10-bit B component in bits 20..29, a 10-bit G component in bits 10..19,
        /// and a 10-bit R component in bits 0..9.
        /// </summary>
        A2B10G10R10UIntPack32 = 68,
        /// <summary>
        /// A four-component, 32-bit packed signed integer format that has a 2-bit A component in
        /// bits 30..31, a 10-bit B component in bits 20..29, a 10-bit G component in bits 10..19,
        /// and a 10-bit R component in bits 0..9.
        /// </summary>
        A2B10G10R10SIntPack32 = 69,
        /// <summary>
        /// A one-component, 16-bit unsigned normalized format that has a single 16-bit R component.
        /// </summary>
        R16UNorm = 70,
        /// <summary>
        /// A one-component, 16-bit signed normalized format that has a single 16-bit R component.
        /// </summary>
        R16SNorm = 71,
        /// <summary>
        /// A one-component, 16-bit unsigned scaled integer format that has a single 16-bit R component.
        /// </summary>
        R16UScaled = 72,
        /// <summary>
        /// A one-component, 16-bit signed scaled integer format that has a single 16-bit R component.
        /// </summary>
        R16SScaled = 73,
        /// <summary>
        /// A one-component, 16-bit unsigned integer format that has a single 16-bit R component.
        /// </summary>
        R16UInt = 74,
        /// <summary>
        /// A one-component, 16-bit signed integer format that has a single 16-bit R component.
        /// </summary>
        R16SInt = 75,
        /// <summary>
        /// A one-component, 16-bit signed floating-point format that has a single 16-bit R component.
        /// </summary>
        R16SFloat = 76,
        /// <summary>
        /// A two-component, 32-bit unsigned normalized format that has a 16-bit R component in bytes
        /// 0..1, and a 16-bit G component in bytes 2..3.
        /// </summary>
        R16G16UNorm = 77,
        /// <summary>
        /// A two-component, 32-bit signed normalized format that has a 16-bit R component in bytes
        /// 0..1, and a 16-bit G component in bytes 2..3.
        /// </summary>
        R16G16SNorm = 78,
        /// <summary>
        /// A two-component, 32-bit unsigned scaled integer format that has a 16-bit R component in
        /// bytes 0..1, and a 16-bit G component in bytes 2..3.
        /// </summary>
        R16G16UScaled = 79,
        /// <summary>
        /// A two-component, 32-bit signed scaled integer format that has a 16-bit R component in
        /// bytes 0..1, and a 16-bit G component in bytes 2..3.
        /// </summary>
        R16G16SScaled = 80,
        /// <summary>
        /// A two-component, 32-bit unsigned integer format that has a 16-bit R component in bytes
        /// 0..1, and a 16-bit G component in bytes 2..3.
        /// </summary>
        R16G16UInt = 81,
        /// <summary>
        /// A two-component, 32-bit signed integer format that has a 16-bit R component in bytes
        /// 0..1, and a 16-bit G component in bytes 2..3.
        /// </summary>
        R16G16SInt = 82,
        /// <summary>
        /// A two-component, 32-bit signed floating-point format that has a 16-bit R component in
        /// bytes 0..1, and a 16-bit G component in bytes 2..3.
        /// </summary>
        R16G16SFloat = 83,
        /// <summary>
        /// A three-component, 48-bit unsigned normalized format that has a 16-bit R component in
        /// bytes 0..1, a 16-bit G component in bytes 2..3, and a 16-bit B component in bytes 4..5.
        /// </summary>
        R16G16B16UNorm = 84,
        /// <summary>
        /// A three-component, 48-bit signed normalized format that has a 16-bit R component in bytes
        /// 0..1, a 16-bit G component in bytes 2..3, and a 16-bit B component in bytes 4..5.
        /// </summary>
        R16G16B16SNorm = 85,
        /// <summary>
        /// A three-component, 48-bit unsigned scaled integer format that has a 16-bit R component in
        /// bytes 0..1, a 16-bit G component in bytes 2..3, and a 16-bit B component in bytes 4..5.
        /// </summary>
        R16G16B16UScaled = 86,
        /// <summary>
        /// A three-component, 48-bit signed scaled integer format that has a 16-bit R component in
        /// bytes 0..1, a 16-bit G component in bytes 2..3, and a 16-bit B component in bytes 4..5.
        /// </summary>
        R16G16B16SScaled = 87,
        /// <summary>
        /// A three-component, 48-bit unsigned integer format that has a 16-bit R component in bytes
        /// 0..1, a 16-bit G component in bytes 2..3, and a 16-bit B component in bytes 4..5.
        /// </summary>
        R16G16B16UInt = 88,
        /// <summary>
        /// A three-component, 48-bit signed integer format that has a 16-bit R component in bytes
        /// 0..1, a 16-bit G component in bytes 2..3, and a 16-bit B component in bytes 4..5.
        /// </summary>
        R16G16B16SInt = 89,
        /// <summary>
        /// A three-component, 48-bit signed floating-point format that has a 16-bit R component in
        /// bytes 0..1, a 16-bit G component in bytes 2..3, and a 16-bit B component in bytes 4..5.
        /// </summary>
        R16G16B16SFloat = 90,
        /// <summary>
        /// A four-component, 64-bit unsigned normalized format that has a 16-bit R component in
        /// bytes 0..1, a 16-bit G component in bytes 2..3, a 16-bit B component in bytes 4..5, and a
        /// 16-bit A component in bytes 6..7.
        /// </summary>
        R16G16B16A16UNorm = 91,
        /// <summary>
        /// A four-component, 64-bit signed normalized format that has a 16-bit R component in bytes
        /// 0..1, a 16-bit G component in bytes 2..3, a 16-bit B component in bytes 4..5, and a
        /// 16-bit A component in bytes 6..7.
        /// </summary>
        R16G16B16A16SNorm = 92,
        /// <summary>
        /// A four-component, 64-bit unsigned scaled integer format that has a 16-bit R component in
        /// bytes 0..1, a 16-bit G component in bytes 2..3, a 16-bit B component in bytes 4..5, and a
        /// 16-bit A component in bytes 6..7.
        /// </summary>
        R16G16B16A16UScaled = 93,
        /// <summary>
        /// A four-component, 64-bit signed scaled integer format that has a 16-bit R component in
        /// bytes 0..1, a 16-bit G component in bytes 2..3, a 16-bit B component in bytes 4..5, and a
        /// 16-bit A component in bytes 6..7.
        /// </summary>
        R16G16B16A16SScaled = 94,
        /// <summary>
        /// A four-component, 64-bit unsigned integer format that has a 16-bit R component in bytes
        /// 0..1, a 16-bit G component in bytes 2..3, a 16-bit B component in bytes 4..5, and a
        /// 16-bit A component in bytes 6..7.
        /// </summary>
        R16G16B16A16UInt = 95,
        /// <summary>
        /// A four-component, 64-bit signed integer format that has a 16-bit R component in bytes
        /// 0..1, a 16-bit G component in bytes 2..3, a 16-bit B component in bytes 4..5, and a
        /// 16-bit A component in bytes 6..7.
        /// </summary>
        R16G16B16A16SInt = 96,
        /// <summary>
        /// A four-component, 64-bit signed floating-point format that has a 16-bit R component in
        /// bytes 0..1, a 16-bit G component in bytes 2..3, a 16-bit B component in bytes 4..5, and a
        /// 16-bit A component in bytes 6..7.
        /// </summary>
        R16G16B16A16SFloat = 97,
        /// <summary>
        /// A one-component, 32-bit unsigned integer format that has a single 32-bit R component.
        /// </summary>
        R32UInt = 98,
        /// <summary>
        /// A one-component, 32-bit signed integer format that has a single 32-bit R component.
        /// </summary>
        R32SInt = 99,
        /// <summary>
        /// A one-component, 32-bit signed floating-point format that has a single 32-bit R component.
        /// </summary>
        R32SFloat = 100,
        /// <summary>
        /// A two-component, 64-bit unsigned integer format that has a 32-bit R component in bytes
        /// 0..3, and a 32-bit G component in bytes 4..7.
        /// </summary>
        R32G32UInt = 101,
        /// <summary>
        /// A two-component, 64-bit signed integer format that has a 32-bit R component in bytes
        /// 0..3, and a 32-bit G component in bytes 4..7.
        /// </summary>
        R32G32SInt = 102,
        /// <summary>
        /// A two-component, 64-bit signed floating-point format that has a 32-bit R component in
        /// bytes 0..3, and a 32-bit G component in bytes 4..7.
        /// </summary>
        R32G32SFloat = 103,
        /// <summary>
        /// A three-component, 96-bit unsigned integer format that has a 32-bit R component in bytes
        /// 0..3, a 32-bit G component in bytes 4..7, and a 32-bit B component in bytes 8..11.
        /// </summary>
        R32G32B32UInt = 104,
        /// <summary>
        /// A three-component, 96-bit signed integer format that has a 32-bit R component in bytes
        /// 0..3, a 32-bit G component in bytes 4..7, and a 32-bit B component in bytes 8..11.
        /// </summary>
        R32G32B32SInt = 105,
        /// <summary>
        /// A three-component, 96-bit signed floating-point format that has a 32-bit R component in
        /// bytes 0..3, a 32-bit G component in bytes 4..7, and a 32-bit B component in bytes 8..11.
        /// </summary>
        R32G32B32SFloat = 106,
        /// <summary>
        /// A four-component, 128-bit unsigned integer format that has a 32-bit R component in bytes
        /// 0..3, a 32-bit G component in bytes 4..7, a 32-bit B component in bytes 8..11, and a
        /// 32-bit A component in bytes 12..15.
        /// </summary>
        R32G32B32A32UInt = 107,
        /// <summary>
        /// A four-component, 128-bit signed integer format that has a 32-bit R component in bytes
        /// 0..3, a 32-bit G component in bytes 4..7, a 32-bit B component in bytes 8..11, and a
        /// 32-bit A component in bytes 12..15.
        /// </summary>
        R32G32B32A32SInt = 108,
        /// <summary>
        /// A four-component, 128-bit signed floating-point format that has a 32-bit R component in
        /// bytes 0..3, a 32-bit G component in bytes 4..7, a 32-bit B component in bytes 8..11, and
        /// a 32-bit A component in bytes 12..15.
        /// </summary>
        R32G32B32A32SFloat = 109,
        /// <summary>
        /// A one-component, 64-bit unsigned integer format that has a single 64-bit R component.
        /// </summary>
        R64UInt = 110,
        /// <summary>
        /// A one-component, 64-bit signed integer format that has a single 64-bit R component.
        /// </summary>
        R64SInt = 111,
        /// <summary>
        /// A one-component, 64-bit signed floating-point format that has a single 64-bit R component.
        /// </summary>
        R64SFloat = 112,
        /// <summary>
        /// A two-component, 128-bit unsigned integer format that has a 64-bit R component in bytes
        /// 0..7, and a 64-bit G component in bytes 8..15.
        /// </summary>
        R64G64UInt = 113,
        /// <summary>
        /// A two-component, 128-bit signed integer format that has a 64-bit R component in bytes
        /// 0..7, and a 64-bit G component in bytes 8..15.
        /// </summary>
        R64G64SInt = 114,
        /// <summary>
        /// A two-component, 128-bit signed floating-point format that has a 64-bit R component in
        /// bytes 0..7, and a 64-bit G component in bytes 8..15.
        /// </summary>
        R64G64SFloat = 115,
        /// <summary>
        /// A three-component, 192-bit unsigned integer format that has a 64-bit R component in bytes
        /// 0..7, a 64-bit G component in bytes 8..15, and a 64-bit B component in bytes 16..23.
        /// </summary>
        R64G64B64UInt = 116,
        /// <summary>
        /// A three-component, 192-bit signed integer format that has a 64-bit R component in bytes
        /// 0..7, a 64-bit G component in bytes 8..15, and a 64-bit B component in bytes 16..23.
        /// </summary>
        R64G64B64SInt = 117,
        /// <summary>
        /// A three-component, 192-bit signed floating-point format that has a 64-bit R component in
        /// bytes 0..7, a 64-bit G component in bytes 8..15, and a 64-bit B component in bytes 16..23.
        /// </summary>
        R64G64B64SFloat = 118,
        /// <summary>
        /// A four-component, 256-bit unsigned integer format that has a 64-bit R component in bytes
        /// 0..7, a 64-bit G component in bytes 8..15, a 64-bit B component in bytes 16..23, and a
        /// 64-bit A component in bytes 24..31.
        /// </summary>
        R64G64B64A64UInt = 119,
        /// <summary>
        /// A four-component, 256-bit signed integer format that has a 64-bit R component in bytes
        /// 0..7, a 64-bit G component in bytes 8..15, a 64-bit B component in bytes 16..23, and a
        /// 64-bit A component in bytes 24..31.
        /// </summary>
        R64G64B64A64SInt = 120,
        /// <summary>
        /// A four-component, 256-bit signed floating-point format that has a 64-bit R component in
        /// bytes 0..7, a 64-bit G component in bytes 8..15, a 64-bit B component in bytes 16..23,
        /// and a 64-bit A component in bytes 24..31.
        /// </summary>
        R64G64B64A64SFloat = 121,
        /// <summary>
        /// A three-component, 32-bit packed unsigned floating-point format that has a 10-bit B
        /// component in bits 22..31, an 11-bit G component in bits 11..21, an 11-bit R component in
        /// bits 0..10. See "fundamentals-fp10" and "fundamentals-fp11".
        /// </summary>
        B10G11R11UFloatPack32 = 122,
        /// <summary>
        /// A three-component, 32-bit packed unsigned floating-point format that has a 5-bit shared
        /// exponent in bits 27..31, a 9-bit B component mantissa in bits 18..26, a 9-bit G component
        /// mantissa in bits 9..17, and a 9-bit R component mantissa in bits 0..8.
        /// </summary>
        E5B9G9R9UFloatPack32 = 123,
        /// <summary>
        /// A one-component, 16-bit unsigned normalized format that has a single 16-bit depth component.
        /// </summary>
        D16UNorm = 124,
        /// <summary>
        /// A two-component, 32-bit format that has 24 unsigned normalized bits in the depth
        /// component and, optionally:, 8 bits that are unused.
        /// </summary>
        X8D24UNormPack32 = 125,
        /// <summary>
        /// A one-component, 32-bit signed floating-point format that has 32-bits in the depth component.
        /// </summary>
        D32SFloat = 126,
        /// <summary>
        /// A one-component, 8-bit unsigned integer format that has 8-bits in the stencil component.
        /// </summary>
        S8UInt = 127,
        /// <summary>
        /// A two-component, 24-bit format that has 16 unsigned normalized bits in the depth
        /// component and 8 unsigned integer bits in the stencil component.
        /// </summary>
        D16UNormS8UInt = 128,
        /// <summary>
        /// A two-component, 32-bit packed format that has 8 unsigned integer bits in the stencil
        /// component, and 24 unsigned normalized bits in the depth component.
        /// </summary>
        D24UNormS8UInt = 129,
        /// <summary>
        /// A two-component format that has 32 signed float bits in the depth component and 8
        /// unsigned integer bits in the stencil component. There are optionally: 24-bits that are unused.
        /// </summary>
        D32SFloatS8UInt = 130,
        /// <summary>
        /// A three-component, block-compressed format where each 64-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGB texel data. This format has no alpha
        /// and is considered opaque.
        /// </summary>
        BC1RgbUNormBlock = 131,
        /// <summary>
        /// A three-component, block-compressed format where each 64-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGB texel data with sRGB nonlinear
        /// encoding. This format has no alpha and is considered opaque.
        /// </summary>
        BC1RgbSRgbBlock = 132,
        /// <summary>
        /// A four-component, block-compressed format where each 64-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGB texel data, and provides 1 bit of alpha.
        /// </summary>
        BC1RgbaUNormBlock = 133,
        /// <summary>
        /// A four-component, block-compressed format where each 64-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGB texel data with sRGB nonlinear
        /// encoding, and provides 1 bit of alpha.
        /// </summary>
        BC1RgbaSRgbBlock = 134,
        /// <summary>
        /// A four-component, block-compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGBA texel data with the first 64 bits
        /// encoding alpha values followed by 64 bits encoding RGB values.
        /// </summary>
        BC2UNormBlock = 135,
        /// <summary>
        /// A four-component, block-compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGBA texel data with the first 64 bits
        /// encoding alpha values followed by 64 bits encoding RGB values with sRGB nonlinear encoding.
        /// </summary>
        BC2SRgbBlock = 136,
        /// <summary>
        /// A four-component, block-compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGBA texel data with the first 64 bits
        /// encoding alpha values followed by 64 bits encoding RGB values.
        /// </summary>
        BC3UNormBlock = 137,
        /// <summary>
        /// A four-component, block-compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGBA texel data with the first 64 bits
        /// encoding alpha values followed by 64 bits encoding RGB values with sRGB nonlinear encoding.
        /// </summary>
        BC3SRgbBlock = 138,
        /// <summary>
        /// A one-component, block-compressed format where each 64-bit compressed texel block encodes
        /// a 4x4 rectangle of unsigned normalized red texel data.
        /// </summary>
        BC4UNormBlock = 139,
        /// <summary>
        /// A one-component, block-compressed format where each 64-bit compressed texel block encodes
        /// a 4x4 rectangle of signed normalized red texel data.
        /// </summary>
        BC4SNormBlock = 140,
        /// <summary>
        /// A two-component, block-compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RG texel data with the first 64 bits
        /// encoding red values followed by 64 bits encoding green values.
        /// </summary>
        BC5UNormBlock = 141,
        /// <summary>
        /// A two-component, block-compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of signed normalized RG texel data with the first 64 bits
        /// encoding red values followed by 64 bits encoding green values.
        /// </summary>
        BC5SNormBlock = 142,
        /// <summary>
        /// A three-component, block-compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned floating-point RGB texel data.
        /// </summary>
        BC6HUFloatBlock = 143,
        /// <summary>
        /// A three-component, block-compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of signed floating-point RGB texel data.
        /// </summary>
        BC6HSFloatBlock = 144,
        /// <summary>
        /// A four-component, block-compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        BC7UNormBlock = 145,
        /// <summary>
        /// A four-component, block-compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        BC7SRgbBlock = 146,
        /// <summary>
        /// A three-component, ETC2 compressed format where each 64-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGB texel data. This format has no alpha
        /// and is considered opaque.
        /// </summary>
        Etc2R8G8B8UNormBlock = 147,
        /// <summary>
        /// A three-component, ETC2 compressed format where each 64-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGB texel data with sRGB nonlinear
        /// encoding. This format has no alpha and is considered opaque.
        /// </summary>
        Etc2R8G8B8SRgbBlock = 148,
        /// <summary>
        /// A four-component, ETC2 compressed format where each 64-bit compressed texel block encodes
        /// a 4x4 rectangle of unsigned normalized RGB texel data, and provides 1 bit of alpha.
        /// </summary>
        Etc2R8G8B8A1UNormBlock = 149,
        /// <summary>
        /// A four-component, ETC2 compressed format where each 64-bit compressed texel block encodes
        /// a 4x4 rectangle of unsigned normalized RGB texel data with sRGB nonlinear encoding, and
        /// provides 1 bit of alpha.
        /// </summary>
        Etc2R8G8B8A1SRgbBlock = 150,
        /// <summary>
        /// A four-component, ETC2 compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGBA texel data with the first 64 bits
        /// encoding alpha values followed by 64 bits encoding RGB values.
        /// </summary>
        Etc2R8G8B8A8UNormBlock = 151,
        /// <summary>
        /// A four-component, ETC2 compressed format where each 64-bit compressed texel block encodes
        /// a 4x4 rectangle of unsigned normalized RGBA texel data with the first 64 bits encoding
        /// alpha values followed by 64 bits encoding RGB values with sRGB nonlinear encoding applied.
        /// </summary>
        Etc2R8G8B8A8SRgbBlock = 152,
        /// <summary>
        /// A one-component, ETC2 compressed format where each 64-bit compressed texel block encodes
        /// a 4x4 rectangle of unsigned normalized red texel data.
        /// </summary>
        EacR11UNormBlock = 153,
        /// <summary>
        /// A one-component, ETC2 compressed format where each 64-bit compressed texel block encodes
        /// a 4x4 rectangle of signed normalized red texel data.
        /// </summary>
        EacR11SNormBlock = 154,
        /// <summary>
        /// A two-component, ETC2 compressed format where each 128-bit compressed texel block encodes
        /// a 4x4 rectangle of unsigned normalized RG texel data with the first 64 bits encoding red
        /// values followed by 64 bits encoding green values.
        /// </summary>
        EacR11G11UNormBlock = 155,
        /// <summary>
        /// A two-component, ETC2 compressed format where each 128-bit compressed texel block encodes
        /// a 4x4 rectangle of signed normalized RG texel data with the first 64 bits encoding red
        /// values followed by 64 bits encoding green values.
        /// </summary>
        EacR11G11SNormBlock = 156,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc4X4UNormBlock = 157,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 4x4 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc4X4SRgbBlock = 158,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 5x4 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc5X4UNormBlock = 159,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 5x4 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc5X4SRgbBlock = 160,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 5x5 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc5X5UNormBlock = 161,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 5x5 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc5X5SRgbBlock = 162,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 6x5 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc6X5UNormBlock = 163,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 6x5 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc6X5SRgbBlock = 164,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 6x6 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc6X6UNormBlock = 165,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 6x6 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc6X6SRgbBlock = 166,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes an 8x5 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc8X5UNormBlock = 167,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes an 8x5 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc8X5SRgbBlock = 168,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes an 8x6 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc8X6UNormBlock = 169,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes an 8x6 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc8X6SRgbBlock = 170,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes an 8x8 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc8X8UNormBlock = 171,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes an 8x8 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc8X8SRgbBlock = 172,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 10x5 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc10X5UNormBlock = 173,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 10x5 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc10X5SRgbBlock = 174,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 10x6 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc10X6UNormBlock = 175,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 10x6 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc10X6SRgbBlock = 176,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 10x8 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc10X8UNormBlock = 177,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 10x8 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc10X8SRgbBlock = 178,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 10x10 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc10X10UNormBlock = 179,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 10x10 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc10X10SRgbBlock = 180,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 12x10 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc12X10UNormBlock = 181,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 12x10 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc12X10SRgbBlock = 182,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 12x12 rectangle of unsigned normalized RGBA texel data.
        /// </summary>
        Astc12X12UNormBlock = 183,
        /// <summary>
        /// A four-component, ASTC compressed format where each 128-bit compressed texel block
        /// encodes a 12x12 rectangle of unsigned normalized RGBA texel data with sRGB nonlinear
        /// encoding applied to the RGB components.
        /// </summary>
        Astc12X12SRgbBlock = 184,
        Pvrtc12BppUNormBlockImg = 1000054000,
        Pvrtc14BppUNormBlockImg = 1000054001,
        Pvrtc22BppUNormBlockImg = 1000054002,
        Pvrtc24BppUNormBlockImg = 1000054003,
        Pvrtc12BppSrgbBlockImg = 1000054004,
        Pvrtc14BppSrgbBlockImg = 1000054005,
        Pvrtc22BppSrgbBlockImg = 1000054006,
        Pvrtc24BppSrgbBlockImg = 1000054007
    }
}