; ModuleID = 'marshal_methods.armeabi-v7a.ll'
source_filename = "marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [247 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [494 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 15721112, ; 2: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 32687329, ; 3: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 223
	i32 34715100, ; 4: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 236
	i32 34839235, ; 5: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 39485524, ; 6: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 42639949, ; 7: System.Threading.Thread => 0x28aa24d => 145
	i32 66541672, ; 8: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 68219467, ; 9: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 82292897, ; 10: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 103324437, ; 11: Sudoku.Avalonia.dll => 0x6289b15 => 244
	i32 117431740, ; 12: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 122350210, ; 13: System.Threading.Channels.dll => 0x74aea82 => 139
	i32 134690465, ; 14: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 240
	i32 142721839, ; 15: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149972175, ; 16: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 159306688, ; 17: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 18: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 207
	i32 176265551, ; 19: System.ServiceProcess => 0xa81994f => 132
	i32 184328833, ; 20: System.ValueTuple.dll => 0xafca281 => 151
	i32 205061960, ; 21: System.ComponentModel => 0xc38ff48 => 18
	i32 220171995, ; 22: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 230752869, ; 23: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 24: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 25: System.Globalization => 0xdd133ce => 42
	i32 246610117, ; 26: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 256616158, ; 27: Avalonia.Themes.Fluent => 0xf4ba6de => 191
	i32 262624092, ; 28: Sudoku.Avalonia.Android.dll => 0xfa7535c => 0
	i32 276479776, ; 29: System.Threading.Timer.dll => 0x107abf20 => 147
	i32 280482487, ; 30: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 219
	i32 291076382, ; 31: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 298918909, ; 32: System.Net.Ping.dll => 0x11d123fd => 69
	i32 318968648, ; 33: Xamarin.AndroidX.Activity.dll => 0x13031348 => 199
	i32 321597661, ; 34: System.Numerics => 0x132b30dd => 83
	i32 338307749, ; 35: MsBox.Avalonia => 0x142a2aa5 => 196
	i32 342366114, ; 36: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 220
	i32 360082299, ; 37: System.ServiceModel.Web => 0x15766b7b => 131
	i32 367780167, ; 38: System.IO.Pipes => 0x15ebe147 => 55
	i32 374914964, ; 39: System.Transactions.Local => 0x1658bf94 => 149
	i32 375677976, ; 40: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 41: System.Threading.Thread.dll => 0x16a510e1 => 145
	i32 385762202, ; 42: System.Memory.dll => 0x16fe439a => 62
	i32 392610295, ; 43: System.Threading.ThreadPool.dll => 0x1766c1f7 => 146
	i32 395744057, ; 44: _Microsoft.Android.Resource.Designer => 0x17969339 => 246
	i32 403441872, ; 45: WindowsBase => 0x180c08d0 => 165
	i32 442565967, ; 46: System.Collections => 0x1a61054f => 12
	i32 450948140, ; 47: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 218
	i32 451504562, ; 48: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 456227837, ; 49: System.Web.HttpUtility.dll => 0x1b317bfd => 152
	i32 459347974, ; 50: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 465846621, ; 51: mscorlib => 0x1bc4415d => 166
	i32 469710990, ; 52: System.dll => 0x1bff388e => 164
	i32 476646585, ; 53: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 219
	i32 497347661, ; 54: Avalonia.Dialogs => 0x1da4ec4d => 176
	i32 498788369, ; 55: System.ObjectModel => 0x1dbae811 => 84
	i32 525008092, ; 56: SkiaSharp.dll => 0x1f4afcdc => 198
	i32 526420162, ; 57: System.Transactions.dll => 0x1f6088c2 => 150
	i32 527452488, ; 58: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 240
	i32 527885601, ; 59: Avalonia.Skia => 0x1f76e521 => 190
	i32 530272170, ; 60: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 540030774, ; 61: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545304856, ; 62: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 63: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 546678719, ; 64: Avalonia.Controls.DataGrid.dll => 0x2095a7bf => 186
	i32 549171840, ; 65: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 577335427, ; 66: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 601371474, ; 67: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 605376203, ; 68: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 613668793, ; 69: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 627609679, ; 70: Xamarin.AndroidX.CustomView => 0x2568904f => 214
	i32 637581149, ; 71: Avalonia.Controls => 0x2600b75d => 174
	i32 639843206, ; 72: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 217
	i32 643868501, ; 73: System.Net => 0x2660a755 => 81
	i32 662205335, ; 74: System.Text.Encodings.Web.dll => 0x27787397 => 136
	i32 662410334, ; 75: Avalonia.Themes.Simple.dll => 0x277b945e => 192
	i32 663517072, ; 76: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 234
	i32 666292255, ; 77: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 205
	i32 672442732, ; 78: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683518922, ; 79: System.Net.Security => 0x28bdabca => 73
	i32 690569205, ; 80: System.Xml.Linq.dll => 0x29293ff5 => 155
	i32 691348768, ; 81: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 242
	i32 693804605, ; 82: System.Windows => 0x295a9e3d => 154
	i32 699345723, ; 83: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 84: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 237
	i32 700358131, ; 85: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 720511267, ; 86: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 241
	i32 722857257, ; 87: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 735137430, ; 88: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 752232764, ; 89: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 759454413, ; 90: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 91: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775507847, ; 92: System.IO.Compression => 0x2e394f87 => 46
	i32 793404064, ; 93: Avalonia.Metal.dll => 0x2f4a62a0 => 179
	i32 795860074, ; 94: Avalonia.MicroCom.dll => 0x2f6fdc6a => 180
	i32 804715423, ; 95: System.Data.Common => 0x2ff6fb9f => 22
	i32 823281589, ; 96: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 97: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 98: System.Xml.XPath.dll => 0x31a103c6 => 160
	i32 834051424, ; 99: System.Net.Quic => 0x31b69d60 => 71
	i32 873119928, ; 100: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 877678880, ; 101: System.Globalization.dll => 0x34505120 => 42
	i32 878954865, ; 102: System.Net.Http.Json => 0x3463c971 => 63
	i32 904024072, ; 103: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 911108515, ; 104: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 928116545, ; 105: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 236
	i32 952186615, ; 106: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 956575887, ; 107: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 241
	i32 967690846, ; 108: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 220
	i32 975236339, ; 109: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 110: System.Xml.XDocument => 0x3a2aaa1d => 158
	i32 986514023, ; 111: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 112: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 992768348, ; 113: System.Collections.dll => 0x3b2c715c => 12
	i32 993180629, ; 114: MsBox.Avalonia.dll => 0x3b32bbd5 => 196
	i32 994442037, ; 115: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 1001831731, ; 116: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1008800730, ; 117: Avalonia.Base.dll => 0x3c2113da => 173
	i32 1012816738, ; 118: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 229
	i32 1019214401, ; 119: System.Drawing => 0x3cbffa41 => 36
	i32 1035644815, ; 120: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 203
	i32 1036536393, ; 121: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1044663988, ; 122: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1052210849, ; 123: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 224
	i32 1052365087, ; 124: Avalonia.OpenGL.dll => 0x3eb9d11f => 181
	i32 1072331303, ; 125: Avalonia.Skia.dll => 0x3fea7a27 => 190
	i32 1082857460, ; 126: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1084122840, ; 127: Xamarin.Kotlin.StdLib => 0x409e66d8 => 238
	i32 1098259244, ; 128: System => 0x41761b2c => 164
	i32 1143774617, ; 129: MicroCom.Runtime.dll => 0x442c9d99 => 197
	i32 1170634674, ; 130: System.Web.dll => 0x45c677b2 => 153
	i32 1175144683, ; 131: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 233
	i32 1204270330, ; 132: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 205
	i32 1208641965, ; 133: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1214827643, ; 134: CommunityToolkit.Mvvm => 0x4868cc7b => 193
	i32 1219128291, ; 135: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1246548578, ; 136: Xamarin.AndroidX.Collection.Jvm.dll => 0x4a4cd262 => 208
	i32 1253011324, ; 137: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260169368, ; 138: Avalonia.Vulkan.dll => 0x4b1ca898 => 182
	i32 1264511973, ; 139: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 230
	i32 1267360935, ; 140: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 232
	i32 1275534314, ; 141: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 242
	i32 1278448581, ; 142: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 202
	i32 1286058215, ; 143: Avalonia.Fonts.Inter => 0x4ca7b0e7 => 188
	i32 1293217323, ; 144: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 215
	i32 1309188875, ; 145: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1324163709, ; 146: DialogHost.Avalonia => 0x4eed227d => 194
	i32 1324164729, ; 147: System.Linq => 0x4eed2679 => 61
	i32 1335329327, ; 148: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1364015309, ; 149: System.IO => 0x514d38cd => 57
	i32 1376866003, ; 150: Xamarin.AndroidX.SavedState => 0x52114ed3 => 229
	i32 1379779777, ; 151: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1397501879, ; 152: MicroCom.Runtime => 0x534c2fb7 => 197
	i32 1402170036, ; 153: System.Configuration.dll => 0x53936ab4 => 19
	i32 1408764838, ; 154: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1411638395, ; 155: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1422545099, ; 156: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1434145427, ; 157: System.Runtime.Handles => 0x557b5293 => 104
	i32 1439761251, ; 158: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1452070440, ; 159: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 160: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1457743152, ; 161: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 162: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1461234159, ; 163: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 164: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1462112819, ; 165: System.IO.Compression.dll => 0x57261233 => 46
	i32 1469204771, ; 166: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 204
	i32 1479771757, ; 167: System.Collections.Immutable => 0x5833866d => 9
	i32 1480492111, ; 168: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 169: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1536373174, ; 170: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1543031311, ; 171: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 138
	i32 1543355203, ; 172: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1550322496, ; 173: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1560592391, ; 174: Avalonia.Controls.DataGrid => 0x5d04c007 => 186
	i32 1565862583, ; 175: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566207040, ; 176: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 141
	i32 1573704789, ; 177: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 178: System.Threading.Overlapped => 0x5e2d7514 => 140
	i32 1592978981, ; 179: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1601112923, ; 180: System.Xml.Serialization => 0x5f6f0b5b => 157
	i32 1604827217, ; 181: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1613762098, ; 182: Avalonia.Base => 0x60300e32 => 173
	i32 1618516317, ; 183: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 184: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 226
	i32 1622358360, ; 185: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1623297518, ; 186: Avalonia.Markup.Xaml.dll => 0x60c18dee => 177
	i32 1624330220, ; 187: Avalonia.Themes.Fluent.dll => 0x60d14fec => 191
	i32 1635184631, ; 188: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 217
	i32 1636350590, ; 189: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 213
	i32 1639515021, ; 190: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 191: System.Text.RegularExpressions => 0x61c036ca => 138
	i32 1641389582, ; 192: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1657153582, ; 193: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 194: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 231
	i32 1675553242, ; 195: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 196: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 197: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679769178, ; 198: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1691477237, ; 199: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696967625, ; 200: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 201: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 239
	i32 1701541528, ; 202: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1726116996, ; 203: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 204: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1744735666, ; 205: System.Transactions.Local.dll => 0x67fe8db2 => 149
	i32 1746316138, ; 206: Mono.Android.Export => 0x6816ab6a => 169
	i32 1750313021, ; 207: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1758240030, ; 208: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1763938596, ; 209: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765942094, ; 210: System.Reflection.Extensions => 0x6942234e => 93
	i32 1776026572, ; 211: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 212: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 213: Mono.Android.Runtime.dll => 0x6a216153 => 170
	i32 1788241197, ; 214: Xamarin.AndroidX.Fragment => 0x6a96652d => 218
	i32 1808609942, ; 215: Xamarin.AndroidX.Loader => 0x6bcd3296 => 226
	i32 1813058853, ; 216: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 238
	i32 1818787751, ; 217: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1824175904, ; 218: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 219: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1832351152, ; 220: Avalonia.Android => 0x6d3775b0 => 184
	i32 1836181115, ; 221: Avalonia.Fonts.Inter.dll => 0x6d71e67b => 188
	i32 1851619785, ; 222: Sudoku => 0x6e5d79c9 => 245
	i32 1858542181, ; 223: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1870277092, ; 224: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1879696579, ; 225: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1885316902, ; 226: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 206
	i32 1888955245, ; 227: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 228: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1898237753, ; 229: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900610850, ; 230: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1910275211, ; 231: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1919016533, ; 232: Xamarin.AndroidX.Core.SplashScreen.dll => 0x7261de55 => 212
	i32 1939592360, ; 233: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1956758971, ; 234: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1983156543, ; 235: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 239
	i32 1994945615, ; 236: Avalonia.Metal => 0x76e8744f => 179
	i32 2011961780, ; 237: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2019465201, ; 238: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 224
	i32 2045470958, ; 239: System.Private.Xml => 0x79eb68ee => 88
	i32 2055257422, ; 240: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 221
	i32 2060060697, ; 241: System.Windows.dll => 0x7aca0819 => 154
	i32 2070888862, ; 242: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2079903147, ; 243: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2090596640, ; 244: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2127167465, ; 245: System.Console => 0x7ec9ffe9 => 20
	i32 2142473426, ; 246: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 247: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 162
	i32 2146852085, ; 248: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2193016926, ; 249: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2201107256, ; 250: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 243
	i32 2201231467, ; 251: System.Net.Http => 0x8334206b => 64
	i32 2203033907, ; 252: Avalonia.MicroCom => 0x834fa133 => 180
	i32 2217644978, ; 253: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 233
	i32 2220367410, ; 254: Xamarin.AndroidX.Core.SplashScreen => 0x84581e32 => 212
	i32 2222056684, ; 255: System.Threading.Tasks.Parallel => 0x8471e4ec => 143
	i32 2252106437, ; 256: System.Xml.Serialization.dll => 0x863c6ac5 => 157
	i32 2256313426, ; 257: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2265110946, ; 258: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2293034957, ; 259: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2295906218, ; 260: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2298471582, ; 261: System.Net.Mail => 0x88ffe49e => 66
	i32 2305521784, ; 262: System.Private.CoreLib.dll => 0x896b7878 => 172
	i32 2309601686, ; 263: Avalonia => 0x89a9b996 => 183
	i32 2315684594, ; 264: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 200
	i32 2320631194, ; 265: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 143
	i32 2340441535, ; 266: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 267: System.ValueTuple => 0x8bbaa2cd => 151
	i32 2353062107, ; 268: System.Net.Primitives => 0x8c40e0db => 70
	i32 2368005991, ; 269: System.Xml.ReaderWriter.dll => 0x8d24e767 => 156
	i32 2378619854, ; 270: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 271: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2384749489, ; 272: Avalonia.Android.dll => 0x8e2463b1 => 184
	i32 2401565422, ; 273: System.Web.HttpUtility => 0x8f24faee => 152
	i32 2403452196, ; 274: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 216
	i32 2408407545, ; 275: Avalonia.Markup.dll => 0x8f8d61f9 => 178
	i32 2421380589, ; 276: System.Threading.Tasks.Dataflow => 0x905355ed => 141
	i32 2435356389, ; 277: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 278: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2454642406, ; 279: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458678730, ; 280: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 281: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2471841756, ; 282: netstandard.dll => 0x93554fdc => 167
	i32 2475788418, ; 283: Java.Interop.dll => 0x93918882 => 168
	i32 2483903535, ; 284: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 285: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2490993605, ; 286: System.AppContext.dll => 0x94798bc5 => 6
	i32 2501346920, ; 287: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2505896520, ; 288: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 223
	i32 2538310050, ; 289: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2562349572, ; 290: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2570120770, ; 291: System.Text.Encodings.Web => 0x9930ee42 => 136
	i32 2581819634, ; 292: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 232
	i32 2585220780, ; 293: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 294: System.Net.Ping => 0x9a20430d => 69
	i32 2589602615, ; 295: System.Threading.ThreadPool => 0x9a5a3337 => 146
	i32 2605712449, ; 296: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 243
	i32 2617129537, ; 297: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 298: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2620871830, ; 299: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 213
	i32 2627185994, ; 300: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2629843544, ; 301: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2663698177, ; 302: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2664396074, ; 303: System.Xml.XDocument.dll => 0x9ecf752a => 158
	i32 2665622720, ; 304: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2668319029, ; 305: Avalonia.Controls.ColorPicker.dll => 0x9f0b5135 => 185
	i32 2676780864, ; 306: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2686887180, ; 307: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2693849962, ; 308: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 309: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 231
	i32 2705935227, ; 310: Avalonia.OpenGL => 0xa1494b7b => 181
	i32 2713243005, ; 311: Avalonia.Remote.Protocol.dll => 0xa1b8cd7d => 189
	i32 2715334215, ; 312: System.Threading.Tasks.dll => 0xa1d8b647 => 144
	i32 2717744543, ; 313: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719963679, ; 314: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2724373263, ; 315: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 316: Xamarin.AndroidX.Activity => 0xa2e0939b => 199
	i32 2735172069, ; 317: System.Threading.Channels => 0xa30769e5 => 139
	i32 2737747696, ; 318: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 204
	i32 2740948882, ; 319: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2748088231, ; 320: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2765824710, ; 321: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 322: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 237
	i32 2775018621, ; 323: Sudoku.Avalonia.Android => 0xa5676c7d => 0
	i32 2778768386, ; 324: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 235
	i32 2779977773, ; 325: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 228
	i32 2803228030, ; 326: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 159
	i32 2819470561, ; 327: System.Xml.dll => 0xa80db4e1 => 163
	i32 2819479764, ; 328: Avalonia.Controls.dll => 0xa80dd8d4 => 174
	i32 2821205001, ; 329: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 330: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 228
	i32 2824502124, ; 331: System.Xml.XmlDocument => 0xa85a7b6c => 161
	i32 2849599387, ; 332: System.Threading.Overlapped.dll => 0xa9d96f9b => 140
	i32 2853208004, ; 333: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 235
	i32 2861098320, ; 334: Mono.Android.Export.dll => 0xaa88e550 => 169
	i32 2875220617, ; 335: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2887636118, ; 336: System.Net.dll => 0xac1dd496 => 81
	i32 2899753641, ; 337: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900621748, ; 338: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 339: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 340: mscorlib.dll => 0xad2a79b6 => 166
	i32 2909740682, ; 341: System.Private.CoreLib => 0xad6f1e8a => 172
	i32 2919462931, ; 342: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2921128767, ; 343: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 201
	i32 2921597873, ; 344: Avalonia.Markup.Xaml => 0xae240bb1 => 177
	i32 2936416060, ; 345: System.Resources.Reader => 0xaf06273c => 98
	i32 2940926066, ; 346: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 347: System.Xml.XPath.XDocument => 0xaf624531 => 159
	i32 2959614098, ; 348: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2968338931, ; 349: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2972252294, ; 350: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2977263743, ; 351: Avalonia.Controls.ColorPicker => 0xb175707f => 185
	i32 2978675010, ; 352: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 215
	i32 2996846495, ; 353: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 222
	i32 3016983068, ; 354: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 230
	i32 3023353419, ; 355: WindowsBase.dll => 0xb434b64b => 165
	i32 3038032645, ; 356: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 246
	i32 3059408633, ; 357: Mono.Android.Runtime => 0xb65adef9 => 170
	i32 3059793426, ; 358: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3075834255, ; 359: System.Threading.Tasks => 0xb755818f => 144
	i32 3082350816, ; 360: Avalonia.Themes.Simple => 0xb7b8f0e0 => 192
	i32 3090735792, ; 361: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3091917316, ; 362: Avalonia.Vulkan => 0xb84aea04 => 182
	i32 3099732863, ; 363: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103600923, ; 364: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3111772706, ; 365: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 366: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 367: System.Threading.Tasks.Extensions => 0xba4127cb => 142
	i32 3132293585, ; 368: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3147165239, ; 369: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3159123045, ; 370: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 371: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3192346100, ; 372: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 373: System.Web => 0xbe592c0c => 153
	i32 3204380047, ; 374: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 375: System.Xml.XmlDocument.dll => 0xbf506931 => 161
	i32 3220365878, ; 376: System.Threading => 0xbff2e236 => 148
	i32 3226221578, ; 377: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3251039220, ; 378: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3265493905, ; 379: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 380: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 142
	i32 3277815716, ; 381: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 382: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 383: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3290767353, ; 384: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3299363146, ; 385: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 386: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3316684772, ; 387: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 388: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 214
	i32 3317144872, ; 389: System.Data => 0xc5b79d28 => 24
	i32 3340387945, ; 390: SkiaSharp => 0xc71a4669 => 198
	i32 3340431453, ; 391: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 206
	i32 3345895724, ; 392: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 227
	i32 3358260929, ; 393: System.Text.Json => 0xc82afec1 => 137
	i32 3362522851, ; 394: Xamarin.AndroidX.Core => 0xc86c06e3 => 210
	i32 3366347497, ; 395: Java.Interop => 0xc8a662e9 => 168
	i32 3395150330, ; 396: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3403906625, ; 397: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3429136800, ; 398: System.Xml => 0xcc6479a0 => 163
	i32 3430777524, ; 399: netstandard => 0xcc7d82b4 => 167
	i32 3445260447, ; 400: System.Formats.Tar => 0xcd5a809f => 39
	i32 3471940407, ; 401: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3476120550, ; 402: Mono.Android => 0xcf3163e6 => 171
	i32 3485117614, ; 403: System.Text.Json.dll => 0xcfbaacae => 137
	i32 3486566296, ; 404: System.Transactions => 0xcfd0c798 => 150
	i32 3493954962, ; 405: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 209
	i32 3497148347, ; 406: Avalonia.Markup => 0xd0723fbb => 178
	i32 3509114376, ; 407: System.Xml.Linq => 0xd128d608 => 155
	i32 3515174580, ; 408: System.Security.dll => 0xd1854eb4 => 130
	i32 3530912306, ; 409: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 410: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3560100363, ; 411: System.Threading.Timer => 0xd432d20b => 147
	i32 3570554715, ; 412: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3598340787, ; 413: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3608519521, ; 414: System.Linq.dll => 0xd715a361 => 61
	i32 3615210680, ; 415: Avalonia.Dialogs.dll => 0xd77bbcb8 => 176
	i32 3624195450, ; 416: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3628015579, ; 417: DialogHost.Avalonia.dll => 0xd83f1fdb => 194
	i32 3633644679, ; 418: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 201
	i32 3638274909, ; 419: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 420: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 221
	i32 3645089577, ; 421: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3660523487, ; 422: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3672681054, ; 423: Mono.Android.dll => 0xdae8aa5e => 171
	i32 3684561358, ; 424: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 209
	i32 3700866549, ; 425: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3706696989, ; 426: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 211
	i32 3716563718, ; 427: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 428: Xamarin.AndroidX.Annotation => 0xdda814c6 => 200
	i32 3732100267, ; 429: System.Net.NameResolution => 0xde7354ab => 67
	i32 3737834244, ; 430: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748608112, ; 431: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3751444290, ; 432: System.Xml.XPath => 0xdf9a7f42 => 160
	i32 3786282454, ; 433: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 207
	i32 3792276235, ; 434: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3792835768, ; 435: HarfBuzzSharp => 0xe21214b8 => 195
	i32 3802395368, ; 436: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3819260425, ; 437: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3823082795, ; 438: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3829621856, ; 439: System.Numerics.dll => 0xe4436460 => 83
	i32 3844307129, ; 440: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 441: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3870376305, ; 442: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3873536506, ; 443: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 444: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3885497537, ; 445: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3888767677, ; 446: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 227
	i32 3896106733, ; 447: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 448: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 210
	i32 3901907137, ; 449: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3910130544, ; 450: Xamarin.AndroidX.Collection.Jvm => 0xe90fdb70 => 208
	i32 3920810846, ; 451: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 452: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 234
	i32 3928044579, ; 453: System.Xml.ReaderWriter => 0xea213423 => 156
	i32 3930554604, ; 454: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3945713374, ; 455: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953953790, ; 456: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 457: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 203
	i32 3959773229, ; 458: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 222
	i32 3991047708, ; 459: Sudoku.Avalonia => 0xede28e1c => 244
	i32 4000935579, ; 460: Avalonia.dll => 0xee796e9b => 183
	i32 4003436829, ; 461: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4003906742, ; 462: HarfBuzzSharp.dll => 0xeea6c4b6 => 195
	i32 4015948917, ; 463: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 202
	i32 4025784931, ; 464: System.Memory => 0xeff49a63 => 62
	i32 4054681211, ; 465: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4063759658, ; 466: Avalonia.Remote.Protocol => 0xf2380d2a => 189
	i32 4068434129, ; 467: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4071413702, ; 468: Avalonia.DesignerSupport => 0xf2acd7c6 => 175
	i32 4073602200, ; 469: System.Threading.dll => 0xf2ce3c98 => 148
	i32 4074166990, ; 470: Avalonia.Diagnostics => 0xf2d6dace => 187
	i32 4099507663, ; 471: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 472: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 473: Xamarin.AndroidX.Emoji2 => 0xf479582c => 216
	i32 4127667938, ; 474: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130442656, ; 475: System.AppContext => 0xf6318da0 => 6
	i32 4147896353, ; 476: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4151237749, ; 477: System.Core => 0xf76edc75 => 21
	i32 4153554407, ; 478: Avalonia.Diagnostics.dll => 0xf79235e7 => 187
	i32 4159265925, ; 479: System.Xml.XmlSerializer => 0xf7e95c85 => 162
	i32 4161255271, ; 480: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164802419, ; 481: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4181436372, ; 482: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 483: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 225
	i32 4185676441, ; 484: System.Security => 0xf97c5a99 => 130
	i32 4196529839, ; 485: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4213026141, ; 486: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4256097574, ; 487: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 211
	i32 4260525087, ; 488: System.Buffers => 0xfdf2741f => 7
	i32 4261741634, ; 489: Avalonia.DesignerSupport.dll => 0xfe050442 => 175
	i32 4264831766, ; 490: Sudoku.dll => 0xfe342b16 => 245
	i32 4274623895, ; 491: CommunityToolkit.Mvvm.dll => 0xfec99597 => 193
	i32 4274976490, ; 492: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4292120959 ; 493: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 225
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [494 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 108, ; 2
	i32 223, ; 3
	i32 236, ; 4
	i32 48, ; 5
	i32 80, ; 6
	i32 145, ; 7
	i32 30, ; 8
	i32 124, ; 9
	i32 102, ; 10
	i32 244, ; 11
	i32 107, ; 12
	i32 139, ; 13
	i32 240, ; 14
	i32 77, ; 15
	i32 124, ; 16
	i32 13, ; 17
	i32 207, ; 18
	i32 132, ; 19
	i32 151, ; 20
	i32 18, ; 21
	i32 26, ; 22
	i32 1, ; 23
	i32 59, ; 24
	i32 42, ; 25
	i32 91, ; 26
	i32 191, ; 27
	i32 0, ; 28
	i32 147, ; 29
	i32 219, ; 30
	i32 54, ; 31
	i32 69, ; 32
	i32 199, ; 33
	i32 83, ; 34
	i32 196, ; 35
	i32 220, ; 36
	i32 131, ; 37
	i32 55, ; 38
	i32 149, ; 39
	i32 74, ; 40
	i32 145, ; 41
	i32 62, ; 42
	i32 146, ; 43
	i32 246, ; 44
	i32 165, ; 45
	i32 12, ; 46
	i32 218, ; 47
	i32 125, ; 48
	i32 152, ; 49
	i32 113, ; 50
	i32 166, ; 51
	i32 164, ; 52
	i32 219, ; 53
	i32 176, ; 54
	i32 84, ; 55
	i32 198, ; 56
	i32 150, ; 57
	i32 240, ; 58
	i32 190, ; 59
	i32 60, ; 60
	i32 51, ; 61
	i32 103, ; 62
	i32 114, ; 63
	i32 186, ; 64
	i32 40, ; 65
	i32 120, ; 66
	i32 52, ; 67
	i32 44, ; 68
	i32 119, ; 69
	i32 214, ; 70
	i32 174, ; 71
	i32 217, ; 72
	i32 81, ; 73
	i32 136, ; 74
	i32 192, ; 75
	i32 234, ; 76
	i32 205, ; 77
	i32 8, ; 78
	i32 73, ; 79
	i32 155, ; 80
	i32 242, ; 81
	i32 154, ; 82
	i32 92, ; 83
	i32 237, ; 84
	i32 45, ; 85
	i32 241, ; 86
	i32 109, ; 87
	i32 129, ; 88
	i32 25, ; 89
	i32 72, ; 90
	i32 55, ; 91
	i32 46, ; 92
	i32 179, ; 93
	i32 180, ; 94
	i32 22, ; 95
	i32 86, ; 96
	i32 43, ; 97
	i32 160, ; 98
	i32 71, ; 99
	i32 3, ; 100
	i32 42, ; 101
	i32 63, ; 102
	i32 16, ; 103
	i32 53, ; 104
	i32 236, ; 105
	i32 105, ; 106
	i32 241, ; 107
	i32 220, ; 108
	i32 34, ; 109
	i32 158, ; 110
	i32 85, ; 111
	i32 32, ; 112
	i32 12, ; 113
	i32 196, ; 114
	i32 51, ; 115
	i32 56, ; 116
	i32 173, ; 117
	i32 229, ; 118
	i32 36, ; 119
	i32 203, ; 120
	i32 35, ; 121
	i32 58, ; 122
	i32 224, ; 123
	i32 181, ; 124
	i32 190, ; 125
	i32 17, ; 126
	i32 238, ; 127
	i32 164, ; 128
	i32 197, ; 129
	i32 153, ; 130
	i32 233, ; 131
	i32 205, ; 132
	i32 29, ; 133
	i32 193, ; 134
	i32 52, ; 135
	i32 208, ; 136
	i32 5, ; 137
	i32 182, ; 138
	i32 230, ; 139
	i32 232, ; 140
	i32 242, ; 141
	i32 202, ; 142
	i32 188, ; 143
	i32 215, ; 144
	i32 85, ; 145
	i32 194, ; 146
	i32 61, ; 147
	i32 112, ; 148
	i32 57, ; 149
	i32 229, ; 150
	i32 99, ; 151
	i32 197, ; 152
	i32 19, ; 153
	i32 111, ; 154
	i32 101, ; 155
	i32 102, ; 156
	i32 104, ; 157
	i32 71, ; 158
	i32 38, ; 159
	i32 32, ; 160
	i32 103, ; 161
	i32 73, ; 162
	i32 9, ; 163
	i32 123, ; 164
	i32 46, ; 165
	i32 204, ; 166
	i32 9, ; 167
	i32 43, ; 168
	i32 4, ; 169
	i32 31, ; 170
	i32 138, ; 171
	i32 92, ; 172
	i32 93, ; 173
	i32 186, ; 174
	i32 49, ; 175
	i32 141, ; 176
	i32 112, ; 177
	i32 140, ; 178
	i32 115, ; 179
	i32 157, ; 180
	i32 76, ; 181
	i32 173, ; 182
	i32 79, ; 183
	i32 226, ; 184
	i32 37, ; 185
	i32 177, ; 186
	i32 191, ; 187
	i32 217, ; 188
	i32 213, ; 189
	i32 64, ; 190
	i32 138, ; 191
	i32 15, ; 192
	i32 116, ; 193
	i32 231, ; 194
	i32 48, ; 195
	i32 70, ; 196
	i32 80, ; 197
	i32 126, ; 198
	i32 94, ; 199
	i32 121, ; 200
	i32 239, ; 201
	i32 26, ; 202
	i32 97, ; 203
	i32 28, ; 204
	i32 149, ; 205
	i32 169, ; 206
	i32 4, ; 207
	i32 98, ; 208
	i32 33, ; 209
	i32 93, ; 210
	i32 21, ; 211
	i32 41, ; 212
	i32 170, ; 213
	i32 218, ; 214
	i32 226, ; 215
	i32 238, ; 216
	i32 2, ; 217
	i32 134, ; 218
	i32 111, ; 219
	i32 184, ; 220
	i32 188, ; 221
	i32 245, ; 222
	i32 58, ; 223
	i32 95, ; 224
	i32 39, ; 225
	i32 206, ; 226
	i32 25, ; 227
	i32 94, ; 228
	i32 89, ; 229
	i32 99, ; 230
	i32 10, ; 231
	i32 212, ; 232
	i32 87, ; 233
	i32 100, ; 234
	i32 239, ; 235
	i32 179, ; 236
	i32 7, ; 237
	i32 224, ; 238
	i32 88, ; 239
	i32 221, ; 240
	i32 154, ; 241
	i32 33, ; 242
	i32 116, ; 243
	i32 82, ; 244
	i32 20, ; 245
	i32 11, ; 246
	i32 162, ; 247
	i32 3, ; 248
	i32 84, ; 249
	i32 243, ; 250
	i32 64, ; 251
	i32 180, ; 252
	i32 233, ; 253
	i32 212, ; 254
	i32 143, ; 255
	i32 157, ; 256
	i32 41, ; 257
	i32 117, ; 258
	i32 131, ; 259
	i32 75, ; 260
	i32 66, ; 261
	i32 172, ; 262
	i32 183, ; 263
	i32 200, ; 264
	i32 143, ; 265
	i32 106, ; 266
	i32 151, ; 267
	i32 70, ; 268
	i32 156, ; 269
	i32 121, ; 270
	i32 127, ; 271
	i32 184, ; 272
	i32 152, ; 273
	i32 216, ; 274
	i32 178, ; 275
	i32 141, ; 276
	i32 20, ; 277
	i32 14, ; 278
	i32 135, ; 279
	i32 75, ; 280
	i32 59, ; 281
	i32 167, ; 282
	i32 168, ; 283
	i32 15, ; 284
	i32 74, ; 285
	i32 6, ; 286
	i32 23, ; 287
	i32 223, ; 288
	i32 91, ; 289
	i32 1, ; 290
	i32 136, ; 291
	i32 232, ; 292
	i32 134, ; 293
	i32 69, ; 294
	i32 146, ; 295
	i32 243, ; 296
	i32 88, ; 297
	i32 96, ; 298
	i32 213, ; 299
	i32 31, ; 300
	i32 45, ; 301
	i32 109, ; 302
	i32 158, ; 303
	i32 35, ; 304
	i32 185, ; 305
	i32 22, ; 306
	i32 114, ; 307
	i32 57, ; 308
	i32 231, ; 309
	i32 181, ; 310
	i32 189, ; 311
	i32 144, ; 312
	i32 118, ; 313
	i32 120, ; 314
	i32 110, ; 315
	i32 199, ; 316
	i32 139, ; 317
	i32 204, ; 318
	i32 54, ; 319
	i32 105, ; 320
	i32 133, ; 321
	i32 237, ; 322
	i32 0, ; 323
	i32 235, ; 324
	i32 228, ; 325
	i32 159, ; 326
	i32 163, ; 327
	i32 174, ; 328
	i32 132, ; 329
	i32 228, ; 330
	i32 161, ; 331
	i32 140, ; 332
	i32 235, ; 333
	i32 169, ; 334
	i32 40, ; 335
	i32 81, ; 336
	i32 56, ; 337
	i32 37, ; 338
	i32 97, ; 339
	i32 166, ; 340
	i32 172, ; 341
	i32 82, ; 342
	i32 201, ; 343
	i32 177, ; 344
	i32 98, ; 345
	i32 30, ; 346
	i32 159, ; 347
	i32 18, ; 348
	i32 127, ; 349
	i32 119, ; 350
	i32 185, ; 351
	i32 215, ; 352
	i32 222, ; 353
	i32 230, ; 354
	i32 165, ; 355
	i32 246, ; 356
	i32 170, ; 357
	i32 16, ; 358
	i32 144, ; 359
	i32 192, ; 360
	i32 125, ; 361
	i32 182, ; 362
	i32 118, ; 363
	i32 38, ; 364
	i32 115, ; 365
	i32 47, ; 366
	i32 142, ; 367
	i32 117, ; 368
	i32 34, ; 369
	i32 95, ; 370
	i32 53, ; 371
	i32 129, ; 372
	i32 153, ; 373
	i32 24, ; 374
	i32 161, ; 375
	i32 148, ; 376
	i32 104, ; 377
	i32 89, ; 378
	i32 60, ; 379
	i32 142, ; 380
	i32 100, ; 381
	i32 5, ; 382
	i32 13, ; 383
	i32 122, ; 384
	i32 135, ; 385
	i32 28, ; 386
	i32 72, ; 387
	i32 214, ; 388
	i32 24, ; 389
	i32 198, ; 390
	i32 206, ; 391
	i32 227, ; 392
	i32 137, ; 393
	i32 210, ; 394
	i32 168, ; 395
	i32 101, ; 396
	i32 123, ; 397
	i32 163, ; 398
	i32 167, ; 399
	i32 39, ; 400
	i32 17, ; 401
	i32 171, ; 402
	i32 137, ; 403
	i32 150, ; 404
	i32 209, ; 405
	i32 178, ; 406
	i32 155, ; 407
	i32 130, ; 408
	i32 19, ; 409
	i32 65, ; 410
	i32 147, ; 411
	i32 47, ; 412
	i32 79, ; 413
	i32 61, ; 414
	i32 176, ; 415
	i32 106, ; 416
	i32 194, ; 417
	i32 201, ; 418
	i32 49, ; 419
	i32 221, ; 420
	i32 14, ; 421
	i32 68, ; 422
	i32 171, ; 423
	i32 209, ; 424
	i32 78, ; 425
	i32 211, ; 426
	i32 108, ; 427
	i32 200, ; 428
	i32 67, ; 429
	i32 63, ; 430
	i32 27, ; 431
	i32 160, ; 432
	i32 207, ; 433
	i32 10, ; 434
	i32 195, ; 435
	i32 11, ; 436
	i32 78, ; 437
	i32 126, ; 438
	i32 83, ; 439
	i32 66, ; 440
	i32 107, ; 441
	i32 65, ; 442
	i32 128, ; 443
	i32 122, ; 444
	i32 77, ; 445
	i32 227, ; 446
	i32 8, ; 447
	i32 210, ; 448
	i32 2, ; 449
	i32 208, ; 450
	i32 44, ; 451
	i32 234, ; 452
	i32 156, ; 453
	i32 128, ; 454
	i32 23, ; 455
	i32 133, ; 456
	i32 203, ; 457
	i32 222, ; 458
	i32 244, ; 459
	i32 183, ; 460
	i32 29, ; 461
	i32 195, ; 462
	i32 202, ; 463
	i32 62, ; 464
	i32 90, ; 465
	i32 189, ; 466
	i32 87, ; 467
	i32 175, ; 468
	i32 148, ; 469
	i32 187, ; 470
	i32 36, ; 471
	i32 86, ; 472
	i32 216, ; 473
	i32 50, ; 474
	i32 6, ; 475
	i32 90, ; 476
	i32 21, ; 477
	i32 187, ; 478
	i32 162, ; 479
	i32 96, ; 480
	i32 50, ; 481
	i32 113, ; 482
	i32 225, ; 483
	i32 130, ; 484
	i32 76, ; 485
	i32 27, ; 486
	i32 211, ; 487
	i32 7, ; 488
	i32 175, ; 489
	i32 245, ; 490
	i32 193, ; 491
	i32 110, ; 492
	i32 225 ; 493
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ df9aaf29a52042a4fbf800daf2f3a38964b9e958"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"min_enum_size", i32 4}
