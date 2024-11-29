Shader "Custom/SphereHighlightTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HighlightColor ("Highlight Color", Color) = (1,0,0,1)
        _MinLat ("Min Latitude", Float) = -90
        _MaxLat ("Max Latitude", Float) = 90
        _MinLon ("Min Longitude", Float) = -180
        _MaxLon ("Max Longitude", Float) = 180
        _Transparency ("Transparency for Non-Highlighted", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        CGPROGRAM
        #pragma surface surf Standard alpha:fade fullforwardshadows

        sampler2D _MainTex;
        fixed4 _HighlightColor;
        float _MinLat;
        float _MaxLat;
        float _MinLon;
        float _MaxLon;
        float _Transparency;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float3 normal = normalize(IN.worldPos);

            // Convert normal to latitude and longitude
            float lat = degrees(asin(normal.y)); // Latitude
            float lon = degrees(atan2(normal.z, normal.x)); // Longitude

            // Determine if the point is within the highlight range
            bool isHighlighted = (lat >= _MinLat && lat <= _MaxLat && lon >= _MinLon && lon <= _MaxLon);

            if (isHighlighted)
            {
                o.Albedo = _HighlightColor.rgb;
                o.Emission = _HighlightColor.rgb;
                o.Alpha = _HighlightColor.a; // Use the alpha from the highlight color
            }
            else
            {
                o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
                o.Alpha = _Transparency; // Apply transparency for non-highlighted areas
            }
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
