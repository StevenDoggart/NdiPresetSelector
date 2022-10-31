/*
    Copied (and combined and modified) from the classes in the Pinvoke folder in the NDILibDonNet2 
    sample project which is prodived in the NewTek NDI SDK
*/

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace NdiCommander;


internal static class NdiLib
{
    /// <summary>
    ///     This REQUIRES you to use Marshal.FreeHGlobal() on the returned pointer!
    /// </summary>
    /// <remarks>
    ///     This is a utility function outside of the NDILib SDK itself, but useful for working with NDI from 
    ///     managed languages.
    /// </remarks>
    public static IntPtr StringToUtf8(String managedString)
    {
        int len = Encoding.UTF8.GetByteCount(managedString);
        byte[] buffer = new byte[len + 1];
        Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
        IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
        Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
        return nativeUtf8;
    }


    /// <summary>
    ///     Length is optional, but recommended.  This is all potentially dangerous.
    /// </summary>
    /// <remarks>
    ///     This is a utility function outside of the NDILib SDK itself, but useful for working with NDI from 
    ///     managed languages.
    /// </remarks>
    public static string Utf8ToString(IntPtr nativeUtf8, uint? length = null)
    {
        if (nativeUtf8 == IntPtr.Zero)
            return String.Empty;
        uint len = 0;
        if (length.HasValue)
        {
            len = length.Value;
        }
        else
        {
            // try to find the terminator
            while (Marshal.ReadByte(nativeUtf8, (int)len) != 0)
                ++len;
        }
        byte[] buffer = new byte[len];
        Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);
        return Encoding.UTF8.GetString(buffer);
    }


    /// <summary>
    ///     Create a new finder instance.  This will return NULL if it fails.  This function ignores the 
    ///     p_extra_ips member and sets it to the default.
    /// </summary>
    public static IntPtr find_create_v2(ref find_create_t p_create_settings)
    {
        if (IntPtr.Size == 8)
            return UnsafeNativeMethods.find_create_v2_64(ref p_create_settings);
        else
            return UnsafeNativeMethods.find_create_v2_32(ref p_create_settings);
    }


    /// <summary>
    ///     This function will recover the current set of sources (i.e. the ones that exist right this second).
    ///     The char* memory buffers returned in source_t are valid until the next call to 
    ///     find_get_current_sources or a call to find_destroy.  For a given find_instance_t, do not call 
    ///     find_get_current_sources asynchronously.
    /// </summary>
    public static IntPtr find_get_current_sources(IntPtr p_instance, ref UInt32 p_no_sources)
    {
        if (IntPtr.Size == 8)
            return UnsafeNativeMethods.find_get_current_sources_64(p_instance, ref p_no_sources);
        else
            return UnsafeNativeMethods.find_get_current_sources_32(p_instance, ref p_no_sources);
    }


    /// <summary>
    ///     This will allow you to wait until the number of online sources have changed
    /// </summary>
    public static bool find_wait_for_sources(IntPtr p_instance, UInt32 timeout_in_ms)
    {
        if (IntPtr.Size == 8)
            return UnsafeNativeMethods.find_wait_for_sources_64(p_instance, timeout_in_ms);
        else
            return UnsafeNativeMethods.find_wait_for_sources_32(p_instance, timeout_in_ms);
    }


    /// <summary>
    ///     Create a new receiver instance. This will return NULL if it fails.
    /// </summary>
    public static IntPtr recv_create_v3(ref recv_create_v3_t p_create_settings)
    {
        if (IntPtr.Size == 8)
            return UnsafeNativeMethods.recv_create_v3_64(ref p_create_settings);
        else
            return UnsafeNativeMethods.recv_create_v3_32(ref p_create_settings);
    }


    /// <summary>
    ///     This will destroy an existing receiver instance
    /// </summary>
    public static void recv_destroy(IntPtr p_instance)
    {
        if (IntPtr.Size == 8)
            UnsafeNativeMethods.recv_destroy_64(p_instance);
        else
            UnsafeNativeMethods.recv_destroy_32(p_instance);
    }


    /// <summary>
    ///     This will allow you to receive video, audio and metadata frames.  Any of the buffers can be NULL, in 
    ///     which case data of that type will not be captured in this call.  This call can be called 
    ///     simultaneously on separate threads, so it is entirely possible to receive audio, video, metadata all 
    ///     on separate threads.  This function will return frame_type_none if no data is received within the 
    ///     specified timeout and frame_type_error if the connection is lost.  Buffers captured with this must 
    ///     be freed with the appropriate free function below.
    /// </summary>
    public static frame_type_e recv_capture_v2(IntPtr p_instance, ref video_frame_v2_t p_video_data, ref audio_frame_v2_t p_audio_data, ref metadata_frame_t p_metadata, UInt32 timeout_in_ms)
    {
        if (IntPtr.Size == 8)
            return UnsafeNativeMethods.recv_capture_v2_64(p_instance, ref p_video_data, ref p_audio_data, ref p_metadata, timeout_in_ms);
        else
            return UnsafeNativeMethods.recv_capture_v2_32(p_instance, ref p_video_data, ref p_audio_data, ref p_metadata, timeout_in_ms);
    }


    /// <summary>
    ///     This will free a string that was allocated and returned by recv function (e.g. the 
    ///     recv_get_web_control)
    /// </summary>
    public static void recv_free_string(IntPtr p_instance, IntPtr p_string)
    {
        if (IntPtr.Size == 8)
            UnsafeNativeMethods.recv_free_string_64(p_instance, p_string);
        else
            UnsafeNativeMethods.recv_free_string_32(p_instance, p_string);
    }


    /// <summary>
    ///     Get the URL that might be used for configuration of this input. Note that it might take a second or 
    ///     two after the connection for this value to be set.  This function will return NULL if there is no 
    ///     web control user interface.  You should call recv_free_string to free the string that is returned by 
    ///     this function.  The returned value will be a fully formed URL 
    ///     (e.g. "http://10.28.1.192/configuration/").  To avoid the need to poll this function, you can know 
    ///     when the value of this function might have changed when the recv_capture* call would return 
    ///     frame_type_status_change.
    /// </summary>
    public static IntPtr recv_get_web_control(IntPtr p_instance)
    {
        if (IntPtr.Size == 8)
            return UnsafeNativeMethods.recv_get_web_control_64(p_instance);
        else
            return UnsafeNativeMethods.recv_get_web_control_32(p_instance);
    }


    /// <summary>
    ///     Has this receiver got PTZ control.  Note that it might take a second or two after the connection for 
    ///     this value to be set.  To avoid the need to poll this function, you can know when the value of this 
    ///     function might have changed when the recv_capture* call would return frame_type_status_change.
    /// </summary>
    public static bool recv_ptz_is_supported(IntPtr p_instance)
    {
        if (IntPtr.Size == 8)
            return UnsafeNativeMethods.recv_ptz_is_supported_64(p_instance);
        else
            return UnsafeNativeMethods.recv_ptz_is_supported_32(p_instance);
    }


    /// <summary>
    ///     Recall a preset, including position, focus, etc...
    ///         speed = 0.0 (as slow as possible) ... 1.0 (as fast as possible) 
    ///     
    /// </summary>
    /// <param name="preset_no">
    ///     A number between 0 and 99, inclusive
    /// </param>
    /// <param name="speed">
    ///     The speed at which to move to the new preset.  A number between 0 and 1, inclusive, where 0 means as 
    ///     slow as possible and 1 means as fast as possible.
    /// </param>
    public static bool recv_ptz_recall_preset(IntPtr p_instance, int preset_no, float speed)
    {
        if (IntPtr.Size == 8)
            return UnsafeNativeMethods.recv_ptz_recall_preset_64(p_instance, preset_no, speed);
        else
            return UnsafeNativeMethods.recv_ptz_recall_preset_32(p_instance, preset_no, speed);
    }


    [SuppressUnmanagedCodeSecurity]
    private static class UnsafeNativeMethods
    {
        // find_create_v2 
        [DllImport("Processing.NDI.Lib.x64.dll", EntryPoint = "NDIlib_find_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr find_create_v2_64(ref find_create_t p_create_settings);
        [DllImport("Processing.NDI.Lib.x86.dll", EntryPoint = "NDIlib_find_create_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr find_create_v2_32(ref find_create_t p_create_settings);


        // find_get_current_sources 
        [DllImport("Processing.NDI.Lib.x64.dll", EntryPoint = "NDIlib_find_get_current_sources", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr find_get_current_sources_64(IntPtr p_instance, ref UInt32 p_no_sources);
        [DllImport("Processing.NDI.Lib.x86.dll", EntryPoint = "NDIlib_find_get_current_sources", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr find_get_current_sources_32(IntPtr p_instance, ref UInt32 p_no_sources);


        // find_wait_for_sources 
        [DllImport("Processing.NDI.Lib.x64.dll", EntryPoint = "NDIlib_find_wait_for_sources", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAsAttribute(UnmanagedType.U1)]
        internal static extern bool find_wait_for_sources_64(IntPtr p_instance, UInt32 timeout_in_ms);
        [DllImport("Processing.NDI.Lib.x86.dll", EntryPoint = "NDIlib_find_wait_for_sources", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAsAttribute(UnmanagedType.U1)]
        internal static extern bool find_wait_for_sources_32(IntPtr p_instance, UInt32 timeout_in_ms);


        // recv_create_v3 
        [DllImport("Processing.NDI.Lib.x64.dll", EntryPoint = "NDIlib_recv_create_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr recv_create_v3_64(ref recv_create_v3_t p_create_settings);
        [DllImport("Processing.NDI.Lib.x86.dll", EntryPoint = "NDIlib_recv_create_v3", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr recv_create_v3_32(ref recv_create_v3_t p_create_settings);


        // recv_destroy 
        [DllImport("Processing.NDI.Lib.x64.dll", EntryPoint = "NDIlib_recv_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void recv_destroy_64(IntPtr p_instance);
        [DllImport("Processing.NDI.Lib.x86.dll", EntryPoint = "NDIlib_recv_destroy", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void recv_destroy_32(IntPtr p_instance);


        // recv_capture_v2 
        [DllImport("Processing.NDI.Lib.x64.dll", EntryPoint = "NDIlib_recv_capture_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern frame_type_e recv_capture_v2_64(IntPtr p_instance, ref video_frame_v2_t p_video_data, ref audio_frame_v2_t p_audio_data, ref metadata_frame_t p_metadata, UInt32 timeout_in_ms);
        [DllImport("Processing.NDI.Lib.x86.dll", EntryPoint = "NDIlib_recv_capture_v2", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern frame_type_e recv_capture_v2_32(IntPtr p_instance, ref video_frame_v2_t p_video_data, ref audio_frame_v2_t p_audio_data, ref metadata_frame_t p_metadata, UInt32 timeout_in_ms);


        // recv_free_string 
        [DllImport("Processing.NDI.Lib.x64.dll", EntryPoint = "NDIlib_recv_free_string", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void recv_free_string_64(IntPtr p_instance, IntPtr p_string);
        [DllImport("Processing.NDI.Lib.x86.dll", EntryPoint = "NDIlib_recv_free_string", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void recv_free_string_32(IntPtr p_instance, IntPtr p_string);


        // recv_get_web_control 
        [DllImport("Processing.NDI.Lib.x64.dll", EntryPoint = "NDIlib_recv_get_web_control", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr recv_get_web_control_64(IntPtr p_instance);
        [DllImport("Processing.NDI.Lib.x86.dll", EntryPoint = "NDIlib_recv_get_web_control", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr recv_get_web_control_32(IntPtr p_instance);


        // recv_ptz_is_supported 
        [DllImport("Processing.NDI.Lib.x64.dll", EntryPoint = "NDIlib_recv_ptz_is_supported", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAsAttribute(UnmanagedType.U1)]
        internal static extern bool recv_ptz_is_supported_64(IntPtr p_instance);
        [DllImport("Processing.NDI.Lib.x86.dll", EntryPoint = "NDIlib_recv_ptz_is_supported", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAsAttribute(UnmanagedType.U1)]
        internal static extern bool recv_ptz_is_supported_32(IntPtr p_instance);


        // recv_ptz_recall_preset 
        [DllImport("Processing.NDI.Lib.x64.dll", EntryPoint = "NDIlib_recv_ptz_recall_preset", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAsAttribute(UnmanagedType.U1)]
        internal static extern bool recv_ptz_recall_preset_64(IntPtr p_instance, int preset_no, float speed);
        [DllImport("Processing.NDI.Lib.x86.dll", EntryPoint = "NDIlib_recv_ptz_recall_preset", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAsAttribute(UnmanagedType.U1)]
        internal static extern bool recv_ptz_recall_preset_32(IntPtr p_instance, int preset_no, float speed);
    }


    /// <summary>
    ///     The creation structure that is used when you are creating a finder
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct find_create_t
    {
        /// <summary>
        ///     Do we want to incluide the list of NDI sources that are running on the local machine?  If TRUE then 
        ///     local sources will be visible, if FALSE then they will not.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool show_local_sources;


        /// <summary>
        ///     Which groups do you want to search in for sources
        /// </summary>
        public IntPtr p_groups;


        /// <summary>
        ///     The list of additional IP addresses that exist that we should query for sources on. For instance, if 
        ///     you want to find the sources on a remote machine that is not on your local sub-net then you can put 
        ///     a comma seperated list of those IP addresses here and those sources will be available locally even 
        ///     though they are not mDNS discoverable. An example might be "12.0.0.8,13.0.12.8".  When none is 
        ///     specified the registry is used.  Default = NULL.
        /// </summary>
        public IntPtr p_extra_ips;
    }


    /// <summary>
    ///     The creation structure that is used when you are creating a receiver
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct recv_create_v3_t
    {
        /// <summary>
        ///     The source that you wish to connect to
        /// </summary>
        public source_t source_to_connect_to;


        /// <summary>
        ///     Your preference of color space. See above.
        /// </summary>
        public recv_color_format_e color_format;


        /// <summary>
        ///     The bandwidth setting that you wish to use for this video source.  Bandwidth controlled by changing 
        ///     both the compression level and the resolution of the source.  A good use for low bandwidth is 
        ///     working on WIFI connections.
        /// </summary>
        public recv_bandwidth_e bandwidth;


        /// <summary>
        ///     When this flag is FALSE, all video that you receive will be progressive.  For sources that provide 
        ///     fields, this is de-interlaced on the receiving side (because we cannot change what the up-stream 
        ///     source was actually rendering.  This is provided as a convenience to down-stream sources that do not 
        ///     wish to understand fielded video.  There is almost no performance impact of using this function.
        /// </summary>
        [MarshalAs(UnmanagedType.U1)]
        public bool allow_video_fields;


        /// <summary>
        ///     The name of the NDI receiver to create.  This is a NULL terminated UTF8 string and should be the 
        ///     name of receive channel that you have.  This is in many ways symetric with the name of senders, so 
        ///     this might be "Channel 1" on your system.
        /// </summary>
        public IntPtr p_ndi_recv_name;
    }


    /// <summary>
    ///     This is a descriptor of a NDI source available on the network
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct source_t
    {
        /// <summary>
        ///     A UTF8 string that provides a user readable name for this source.  This can be used for 
        ///     serialization, etc... and comprises the machine name and the source name on that machine.  In the 
        ///     form MACHINE_NAME (NDI_SOURCE_NAME).  If you specify this parameter either as NULL, or an EMPTY 
        ///     string then the specific IP addres adn port number from below is used.
        /// </summary>
        public IntPtr p_ndi_name;


        /// <summary>
        ///     A UTF8 string that provides the actual network address and any parameters.  This is not meant to be 
        ///     application readable and might well change in the future.  This can be nullptr if you do not know it 
        ///     and the API internally will instantiate a finder that is used to discover it even if it is not yet 
        ///     available on the network.
        /// </summary>
        public IntPtr p_url_address;
    }


    /// <summary>
    ///     This describes a video frame
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct video_frame_v2_t
    {
        /// <summary>
        ///     The resolution of this frame
        /// </summary>
        public int xres, yres;


        /// <summary>
        ///     What FourCC this is with. This can be two values
        /// </summary>
        public FourCC_type_e FourCC;


        /// <summary>
        ///     What is the frame-rate of this frame.  For instance NTSC is 30000,1001 = 30000/1001 = 29.97fps.
        /// </summary>
        public int frame_rate_N, frame_rate_D;


        /// <summary>
        ///     What is the picture aspect ratio of this frame.  For instance 16.0/9.0 = 1.778 is 16:9 video.  
        ///     0 means square pixels.
        /// </summary>
        public float picture_aspect_ratio;


        /// <summary>
        ///     Is this a fielded frame, or is it progressive
        /// </summary>
        public frame_format_type_e frame_format_type;


        /// <summary>
        ///     The timecode of this frame in 100ns intervals
        /// </summary>
        public Int64 timecode;


        /// <summary>
        ///     The video data itself
        /// </summary>
        public IntPtr p_data;


        /// <summary>
        ///     The inter line stride of the video data, in bytes
        /// </summary>
        public int line_stride_in_bytes;


        /// <summary>
        ///     Per frame metadata for this frame.  This is a NULL terminated UTF8 string that should be in XML 
        ///     format.  If you do not want any metadata then you may specify NULL here.
        /// </summary>
        public IntPtr p_metadata;


        /// <summary>
        ///     This is only valid when receiving a frame and is specified as a 100ns time that was the exact moment 
        ///     that the frame was submitted by the sending side and is generated by the SDK.  If this value is 
        ///     recv_timestamp_undefined then this value is not available and is recv_timestamp_undefined.
        /// </summary>
        public Int64 timestamp;
    }


    /// <summary>
    ///     This describes an audio frame
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct audio_frame_v2_t
    {
        /// <summary>
        ///     The sample-rate of this buffer
        /// </summary>
        public int sample_rate;


        /// <summary>
        ///     The number of audio channels
        /// </summary>
        public int no_channels;


        /// <summary>
        ///     The number of audio samples per channel
        /// </summary>
        public int no_samples;


        /// <summary>
        ///     The timecode of this frame in 100ns intervals
        /// </summary>
        public Int64 timecode;


        /// <summary>
        ///     The audio data
        /// </summary>
        public IntPtr p_data;


        /// <summary>
        ///     The inter channel stride of the audio channels, in bytes
        /// </summary>
        public int channel_stride_in_bytes;


        /// <summary>
        ///     Per frame metadata for this frame.  This is a NULL terminated UTF8 string that should be in XML 
        ///     format.  If you do not want any metadata then you may specify NULL here.
        /// </summary>
        public IntPtr p_metadata;


        /// <summary>
        ///     This is only valid when receiving a frame and is specified as a 100ns time that was the exact moment 
        ///     that the frame was submitted by the sending side and is generated by the SDK.  If this value is 
        ///     recv_timestamp_undefined then this value is not available and is recv_timestamp_undefined.
        /// </summary>
        public Int64 timestamp;
    }


    /// <summary>
    ///     The data description for metadata
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct metadata_frame_t
    {
        /// <summary>
        ///     The length of the string in UTF8 characters.  This includes the NULL terminating character.  If this 
        ///     is 0, then the length is assume to be the length of a NULL terminated string.
        /// </summary>
        public int length;


        /// <summary>
        ///     The timecode of this frame in 100ns intervals
        /// </summary>
        public Int64 timecode;


        /// <summary>
        ///     The metadata as a UTF8 XML string.  This is a NULL terminated string.
        /// </summary>
        public IntPtr p_data;
    }


    /// <summary>
    ///     An enumeration to specify the type of a packet returned by the functions
    /// </summary>
    public enum frame_type_e
    {
        none = 0,
        video = 1,
        audio = 2,
        metadata = 3,
        error = 4,


        /// <summary>
        ///     This indicates that the settings on this input have changed.  For instamce, this value will be 
        ///     returned from recv_capture_v2 and recv_capture when the device is known to have new settings, for 
        ///     instance the web-url has changed ot the device is now known to be a PTZ camera.
        /// </summary>
        status_change = 100
    }


    public enum FourCC_type_e
    {
        /// <summary>
        ///     YCbCr color space
        /// </summary>
        UYVY = 0x59565955,


        /// <summary>
        ///     4:2:0 format
        /// </summary>
        YV12 = 0x32315659,


        /// <summary>
        ///     4:2:0 format
        /// </summary>
        NV12 = 0x3231564E,


        /// <summary>
        ///     4:2:0 format
        /// </summary>
        I420 = 0x30323449,


        /// <summary>
        ///     BGRA
        /// </summary>
        BGRA = 0x41524742,


        /// <summary>
        ///     BGRA
        /// </summary>
        BGRX = 0x58524742,


        /// <summary>
        ///     RGBA
        /// </summary>
        RGBA = 0x41424752,


        /// <summary>
        ///     RGBA
        /// </summary>
        RGBX = 0x58424752,


        /// <summary>
        ///     This is a UYVY buffer followed immediately by an alpha channel buffer.  If the stride of the YCbCr 
        ///     component is "stride", then the alpha channel starts at image_ptr + yres*stride. The alpha channel 
        ///     stride is stride / 2.
        /// <summary>
        UYVA = 0x41565955
    }


    public enum frame_format_type_e
    {
        /// <summary>
        ///     A progressive frame
        /// </summary>
        progressive = 1,


        /// <summary>
        ///     A fielded frame with the field 0 being on the even lines and field 1 being on the odd lines
        /// </summary>
        interleaved = 0,


        /// <summary>
        ///     Individual fields
        /// </summary>
        field_0 = 2,
        field_1 = 3
    }


    public enum recv_bandwidth_e
    {
        /// <summary>
        ///     Receive metadata
        /// </summary>
        metadata_only = -10,


        /// <summary>
        ///     Receive metadata audio
        /// </summary>
        audio_only = 10,


        /// <summary>
        ///     Receive metadata audio video at a lower bandwidth and resolution
        /// </summary>
        lowest = 0,


        /// <summary>
        ///     Receive metadata audio video at full resolution
        /// </summary>
        highest = 100
    }


    public enum recv_color_format_e
    {
        /// <summary>
        ///     No alpha channel: BGRX Alpha channel: BGRA
        /// </summary>
        BGRX_BGRA = 0,


        /// <summary>
        ///     No alpha channel: UYVY Alpha channel: BGRA
        /// </summary>
        UYVY_BGRA = 1,


        /// <summary>
        ///     No alpha channel: RGBX Alpha channel: RGBA
        /// </summary>
        RGBX_RGBA = 2,


        /// <summary>
        ///     No alpha channel: UYVY Alpha channel: RGBA
        /// </summary>
        UYVY_RGBA = 3,


        /// <summary>
        ///     On Windows there are some APIs that require bottom to top images in RGBA format.  Specifying this 
        ///     format will return images in this format. The image data pointer will still point to the "top" of 
        ///     the image, althought he stride will be negative.  You can get the "bottom" line of the image using: 
        ///     video_data.p_data + (video_data.yres - 1) * video_data.line_stride_in_bytes
        /// </summary>
        BGRX_BGRA_flipped = 200,


        /// <summary>
        ///     Read the SDK documentation to understand the pros and cons of this format
        /// </summary>
        fastest = 100,


        /// <summary>
        ///     Legacy definition for backwards compatibility
        /// </summary>
        e_BGRX_BGRA = BGRX_BGRA,


        /// <summary>
        ///     Legacy definition for backwards compatibility
        /// </summary>
        e_UYVY_BGRA = UYVY_BGRA,


        /// <summary>
        ///     Legacy definition for backwards compatibility
        /// </summary>
        e_RGBX_RGBA = RGBX_RGBA,


        /// <summary>
        ///     Legacy definition for backwards compatibility
        /// </summary>
        e_UYVY_RGBA = UYVY_RGBA
    }
}