using UnityEngine;
using System.Collections;

// FPS Graph - Performance Analyzer Tool - Version 0.975
//
// To use FPS Graph simply add this script to your main camera. 
//    detailed explanation: Select the camera used in your scene then go to the inspector window click on the component menu and down to scripts you should find FPSGraph 
// Options:
//    Audio Feedback: This allows you to audibly "visualize" the perforamnce of the scene
//    Audio Feedback Volume: This specifies how loud the audio feedback is, from 0.0-1.0
//    Graph Multiply: This specifies how zoomed in the graph is, the default is the graph is multiplyed by 2x, meaning every pixel is doubled.
//    Graph Position: This specifies where the graph sits on the screen examples: x:0.0, y:0.0 (top-left) x:1.0, y:0.0 (top-right) x:0.0, y:1.0 (bottom-left) x:1.0 y:1.0 (bottom-right)
//    Frame History Length: This is the length of FPS that is displayed (Set this to a low amount if you are targeting older mobile devices)
public class FPSGraphC : MonoBehaviour
{
	Material mat;

	public void CreateLineMaterial() {
	    mat = new Material(Shader.Find("GUI/Text Shader") );
	}

	public bool showPerformanceOnClick = true;
	public bool showFPSNumber = false;
	public bool audioFeedback = false;
	public float audioFeedbackVolume = 0.5f;
	public int graphMultiply = 2;
	public Vector2 graphPosition = new Vector2(0.0f, 0.0f);
	public int frameHistoryLength = 120;

	public Color CpuColor = new Color( 53.0f/ 255.0f, 136.0f / 255.0f, 167.0f / 255.0f , 1.0f );
	public Color RenderColor = new Color( 112.0f/ 255.0f, 156.0f / 255.0f, 6.0f / 255.0f, 1.0f );
	public Color OtherColor = new Color( 193.0f/ 255.0f, 108.0f / 255.0f, 1.0f / 255.0f, 1.0f );

	public bool useMinFPS = false;
	public int minFPS = 20;

	// Re-created Assets
	readonly int[] numberBits = new[] {1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,0,0,1,0,1,1,1,0,1,1,1,0,0,1,0,0,1,1,1,0,0,0,1,1,0,1,0,0,1,0,0,0,1,0,0,0,0,1,0,0,0,1,0,0,0,1,0,1,0,1,0,0,1,0,0,1,0,1,0,0,0,1,1,0,1,0,0,1,0,0,0,0,1,0,0,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,0,0,1,0,1,1,1,0,1,1,1,1,0,1,0,1,1,0,0,1,0,1,0,0,0,1,0,1,0,1,0,1,0,0,0,1,0,0,0,0,0,1,0,1,0,1,0,1,0,1,1,1,1,0,0,1,0,0,0,1,0,0,1,1,1,0,0,0,1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1,0,1,1,1};
	readonly int[] fpsBits = new[] {1,0,0,0,1,0,0,0,1,1,1,1,1,0,0,1,1,1,0,0,1,1,1,0,0,0,1,0,1,0,1,1,0,0,1,1,0,1,1,1,0,1,1,1};
	readonly int[] mbBits = new[] {1,0,1,0,1,0,1,1,1,1,0,1,0,1,0,1,0,1,1,1,1,1,1,0,1,1,1,0,0,0,0,0,0,1,0,0};
	readonly float[] bNote = new[] {0.01300049f,0.02593994f,0.03900146f,0.03894043f,0.05194092f,0.06494141f,0.06494141f,0.06494141f,0.07791138f,0.09091187f,0.07791138f,0.09091187f,0.1039124f,0.1038818f,0.1168823f,0.1168823f,0.1298828f,0.1298218f,0.1429443f,0.1427612f,0.1429443f,0.1687622f,0.1688843f,0.1687927f,0.1818237f,0.1818542f,0.1947327f,0.1949158f,0.2206421f,0.2079163f,0.2206726f,0.2208557f,0.2337036f,0.2208252f,0.2207642f,0.2337646f,0.2467651f,0.2337341f,0.2597656f,0.2467651f,0.2597046f,0.2727661f,0.2597046f,0.2597656f,0.2857361f,0.2856445f,0.2857666f,0.2986755f,0.2987061f,0.3117065f,0.311676f,0.324646f,0.3247375f,0.324585f,0.3507385f,0.337616f,0.350647f,0.363678f,0.3765564f,0.363678f,0.3766174f,0.3765869f,0.389679f,0.4025269f,0.4286499f,0.4284668f,0.4286499f,0.4544983f,0.4545898f,0.4674988f,0.4675293f,0.4805603f,0.4804382f,0.5065918f,0.5194092f,0.5195007f,0.5195007f,0.5324097f,0.5455322f,0.5583801f,0.5585022f,0.5713501f,0.5715027f,0.5713806f,0.5844421f,0.5974121f,0.5973511f,0.6104431f,0.6103821f,0.6233521f,0.6364136f,0.649292f,0.6493835f,0.6493225f,0.662384f,0.6752625f,0.6753845f,0.6882935f,0.7012634f,0.6883545f,0.7012634f,0.7142944f,0.7273254f,0.7272034f,0.7402954f,0.7402344f,0.7402649f,0.7402954f,0.7532043f,0.7532654f,0.7792053f,0.7792358f,0.7792053f,0.7792358f,0.7922058f,0.7921753f,0.7922668f,0.8051147f,0.8052979f,0.7921143f,0.7922668f,0.8051758f,0.8051453f,0.7922974f,0.7921143f,0.8182678f,0.8051453f,0.8182068f,0.8051758f,0.7922058f,0.8052063f,0.7922058f,0.7922058f,0.7662354f,0.7662354f,0.7532043f,0.7403564f,0.7401428f,0.7403259f,0.7142944f,0.7012024f,0.7014771f,0.6751404f,0.6494751f,0.662262f,0.6364136f,0.6103516f,0.6104126f,0.6103821f,0.5844116f,0.5714111f,0.5584717f,0.5454102f,0.5325317f,0.5324097f,0.5195313f,0.5064392f,0.4935303f,0.4805298f,0.4675293f,0.4545593f,0.4674988f,0.4675598f,0.4544983f,0.4416504f,0.4544373f,0.4546509f,0.4284668f,0.4416504f,0.4285278f,0.4285583f,0.4286194f,0.4284973f,0.4286499f,0.4415283f,0.4285583f,0.4285889f,0.4545288f,0.4415588f,0.4415894f,0.4415283f,0.4675293f,0.4545898f,0.4804382f,0.4676208f,0.4674683f,0.4805298f,0.4805298f,0.5065308f,0.4934082f,0.5066223f,0.5323181f,0.5455627f,0.5454102f,0.5584717f,0.5584412f,0.5713806f,0.5714722f,0.5713806f,0.5974731f,0.5973511f,0.5974121f,0.6234131f,0.5973206f,0.5975037f,0.623291f,0.6234131f,0.6233826f,0.6233521f,0.6493835f,0.6233521f,0.6363525f,0.6234131f,0.6233215f,0.6364441f,0.636261f,0.6364441f,0.636322f,0.6104126f,0.6363525f,0.6104126f,0.610321f,0.6234436f,0.6103516f,0.5974121f,0.6104431f,0.623291f,0.6104736f,0.623291f,0.6104736f,0.6233215f,0.5974121f,0.6234436f,0.61026f,0.6105347f,0.5972595f,0.6235046f,0.6102905f,0.5974731f,0.5973511f,0.6104431f,0.5843506f,0.5844727f,0.5843811f,0.5844421f,0.5714111f,0.5714722f,0.5713501f,0.5585327f,0.5583496f,0.5585022f,0.5324707f,0.5454407f,0.5194702f,0.5194702f,0.4935303f,0.5064697f,0.4935608f,0.4934082f,0.4806213f,0.4804077f,0.4806519f,0.4674072f,0.4676208f,0.4674683f,0.4935608f,0.4804993f,0.4804993f,0.4675903f,0.4674377f,0.4676514f,0.4544373f,0.4546204f,0.4545288f,0.4674988f,0.4675903f,0.4544678f,0.4546509f,0.4544373f,0.4416199f,0.4544983f,0.4415894f,0.4415894f,0.4414978f,0.4545898f,0.4284973f,0.4546509f,0.4414673f,0.4545898f,0.4415894f,0.4544678f,0.4416504f,0.4414673f,0.4286499f,0.4414978f,0.4286499f,0.4284668f,0.4286804f,0.4414368f,0.4287109f,0.4284363f,0.4286804f,0.4154968f,0.4286194f,0.4285583f,0.4155884f,0.4025879f,0.4026184f,0.3895874f,0.4026184f,0.4025574f,0.3766479f,0.3896179f,0.3766174f,0.3896484f,0.3635254f,0.3637695f,0.3375244f,0.3507996f,0.3245544f,0.3247681f,0.311615f,0.3117065f,0.2857361f,0.285675f,0.2598267f,0.2596436f,0.2468262f,0.2207336f,0.2077942f,0.2208252f,0.1947327f,0.1818848f,0.1687927f,0.1558533f,0.1428528f,0.1298828f,0.1038513f,0.1169128f,0.09091187f,0.09088135f,0.06500244f,0.06484985f,0.06500244f,0.0519104f,0.03897095f,0.02600098f,0.02593994f,0.01300049f,3.051758E-05f,-9.155273E-05f,-0.01287842f,-9.155273E-05f,-0.02590942f,-0.02603149f,-0.03890991f,-0.03900146f,-0.03894043f,-0.03897095f,-0.05194092f,-0.06494141f,-0.05194092f,-0.07794189f,-0.07791138f,-0.07791138f,-0.09091187f,-0.1039429f,-0.1038513f,-0.1168823f,-0.1168823f,-0.1299133f,-0.1427917f,-0.1429443f,-0.1557312f,-0.1429443f,-0.1687622f,-0.1689148f,-0.1817017f,-0.1949463f,-0.1946716f,-0.2078857f,-0.2337341f,-0.2207642f,-0.2207947f,-0.2467651f,-0.2467346f,-0.2338257f,-0.2856445f,-0.2987671f,-0.2986145f,-0.3117676f,-0.3116455f,-0.3117371f,-0.337616f,-0.3506775f,-0.337616f,-0.350708f,-0.3506165f,-0.3636169f,-0.4026794f,-0.4024658f,-0.3897705f,-0.3894653f,-0.4026794f,-0.4155884f,-0.4155273f,-0.4286499f,-0.4284973f,-0.4415894f,-0.4545593f,-0.4545288f,-0.4415283f,-0.4416199f,-0.4544983f,-0.4545593f,-0.4675598f,-0.4674683f,-0.4675903f,-0.4674988f,-0.4805603f,-0.4674988f,-0.4805298f,-0.4675293f,-0.4804993f,-0.4805908f,-0.4804382f,-0.4805908f,-0.4804382f,-0.4675903f,-0.4675293f,-0.4674988f,-0.4675903f,-0.4674377f,-0.4676208f,-0.4415283f,-0.4545288f,-0.4545898f,-0.4674683f,-0.4416199f,-0.4415283f,-0.4285889f,-0.4285583f,-0.4155884f,-0.4026184f,-0.4155884f,-0.4025269f,-0.3897095f,-0.3894958f,-0.3767395f,-0.3765564f,-0.350647f,-0.363678f,-0.3635864f,-0.3377075f,-0.337616f,-0.3377075f,-0.311676f,-0.311676f,-0.2987366f,-0.2856445f,-0.2728271f,-0.2726135f,-0.2598267f,-0.2596741f,-0.2338257f,-0.2207336f,-0.2078247f,-0.2077332f,-0.2078857f,-0.1947021f,-0.1949158f,-0.1687317f,-0.1689148f,-0.1557922f,-0.1558838f,-0.1427917f,-0.1299744f,-0.1167908f,-0.1169434f,-0.09085083f,-0.1039429f,-0.09088135f,-0.07794189f,-0.06491089f,-0.05197144f,-0.03894043f,-0.02600098f,-0.01293945f,-0.02603149f,-0.01296997f,3.051758E-05f,0.01293945f,0.01306152f,0.03887939f,0.03900146f,0.03894043f,0.03897095f,0.06497192f,0.06488037f,0.05200195f,0.07785034f,0.0909729f,0.07788086f,0.1039124f,0.09091187f,0.09091187f,0.09088135f,0.1039124f,0.1039124f,0.1298218f,0.1169434f,0.1298218f,0.1298828f,0.1169128f,0.1298218f,0.1428833f,0.1169128f,0.1297913f,0.1299744f,0.1427612f,0.1169434f,0.1298523f,0.1558228f,0.1558838f,0.1298523f,0.1428528f,0.1428528f,0.1298828f,0.1298523f,0.1169128f,0.1298523f,0.1168518f,0.1169434f,0.1038513f,0.1169128f,0.1038818f,0.1168823f,0.09088135f,0.1169434f,0.09085083f,0.0909729f,0.1038208f,0.1039734f,0.07785034f,0.0909729f,0.07785034f,0.0909729f,0.07788086f,0.07797241f,0.06484985f,0.0909729f,0.06491089f,0.06494141f,0.07794189f,0.07785034f,0.07803345f,0.07781982f,0.0909729f,0.1038513f,0.1039429f,0.1038818f,0.1168823f,0.1168823f,0.1168518f,0.1299438f,0.1427917f,0.1428833f,0.1558533f,0.1558228f,0.1558533f,0.1688232f,0.1818237f,0.1817932f,0.1948547f,0.1947632f,0.2207947f,0.2207642f,0.2337646f,0.2337952f,0.2597046f,0.2337952f,0.2597351f,0.2727051f,0.2727661f,0.2726746f,0.2987671f,0.285675f,0.2987061f,0.2987061f,0.311676f,0.324707f,0.3376465f,0.324646f,0.3247375f,0.3246155f,0.3377075f,0.3376465f,0.350647f,0.337677f,0.3376465f,0.3506775f,0.337616f,0.363678f,0.3506165f,0.3506775f,0.337677f,0.337616f,0.350708f,0.324585f,0.337738f,0.324646f,0.3246765f,0.2857361f,0.2986755f,0.2857056f,0.2727661f,0.2726746f,0.2727661f,0.2597351f,0.2467651f,0.2467346f,0.2337646f,0.2207947f,0.2207642f,0.2078552f,0.2206726f,0.1949158f,0.1817017f,0.1819458f,0.1687317f,0.1688843f,0.1688232f,0.1557922f,0.1559448f,0.1557312f,0.1559448f,0.1427917f,0.1558838f,0.1557922f,0.1429443f,0.1557617f,0.1558838f,0.1558228f,0.1688538f,0.1688232f,0.1948242f,0.1947632f,0.2078247f,0.2207947f,0.2337036f,0.2338867f,0.246582f,0.2729187f,0.2855835f,0.3117371f,0.3246765f,0.337616f,0.350708f,0.3895874f,0.3766174f,0.4155884f,0.4285583f,0.4545898f,0.4674683f,0.4805603f,0.4935303f,0.5324097f,0.5455322f,0.5583496f,0.5844727f,0.5974121f,0.6233215f,0.6364441f,0.6622314f,0.6754456f,0.7011719f,0.701416f,0.7142029f,0.7273254f,0.7532349f,0.7532349f,0.7532654f,0.7791748f,0.7792969f,0.7921448f,0.8182373f,0.8051147f,0.8052673f,0.8181458f,0.8312073f,0.8311157f,0.8181763f,0.8182068f,0.8312073f,0.8181152f,0.8052673f,0.8050842f,0.8052979f,0.7921143f,0.8052979f,0.7921143f,0.7922974f,0.7791443f,0.7662354f,0.7662659f,0.7662048f,0.7532959f,0.7402344f,0.7142334f,0.7143555f,0.7272034f,0.7013855f,0.7012329f,0.688324f,0.688324f,0.675293f,0.662384f,0.6493225f,0.6493225f,0.6364441f,0.636261f,0.6235046f,0.62323f,0.6235046f,0.59729f,0.5975342f,0.61026f,0.5975037f,0.5973206f,0.5844727f,0.5844116f,0.5973511f,0.5845032f,0.5713196f,0.5715332f,0.5713806f,0.5584412f,0.5584717f,0.5583801f,0.5455322f,0.5454102f,0.5454712f,0.5324707f,0.5324402f,0.5065308f,0.5194397f,0.5065308f,0.4934692f,0.4805603f,0.4934692f,0.4675598f,0.4415283f,0.4545898f,0.4545288f,0.4415283f,0.4285889f,0.4285889f,0.4025574f,0.3897095f,0.3894653f,0.3767395f,0.3765564f,0.3636475f,0.3506775f,0.3376465f,0.3246765f,0.3246765f,0.311676f,0.2987061f,0.2857361f,0.2727051f,0.2727356f,0.2727356f,0.2467041f,0.2597961f,0.2597046f,0.2597656f,0.2467651f,0.2467041f,0.2337952f,0.2337646f,0.2467651f,0.2337341f,0.2468262f,0.2596436f,0.2598267f,0.2596741f,0.2727966f,0.2726746f,0.2727661f,0.285675f,0.2987061f,0.2987366f,0.2986755f,0.324707f,0.324646f,0.3376465f,0.3377075f,0.3505859f,0.3507385f,0.3505554f,0.3637085f,0.3635864f,0.363678f,0.3635864f,0.3766479f,0.3636475f,0.3506165f,0.363678f,0.3506165f,0.3376465f,0.3377075f,0.3505859f,0.3247375f,0.324646f,0.2987061f,0.2857361f,0.2466736f,0.2728577f,0.2466125f,0.2209167f,0.2076721f,0.1819153f,0.1557617f,0.1559143f,0.1168213f,0.1039429f,0.07788086f,0.03900146f,0.01293945f,0.01303101f,-0.03897095f,-0.06497192f,-0.09085083f,-0.1169434f,-0.1298218f,-0.1558533f,-0.1948242f,-0.2077637f,-0.2207947f,-0.2597351f,-0.2727356f,-0.2987061f,-0.3116455f,-0.3377075f,-0.337616f,-0.363678f,-0.3636475f,-0.3895569f,-0.4026489f,-0.4155579f,-0.4155579f,-0.4286499f,-0.4414978f,-0.4415894f,-0.4415283f,-0.4545593f,-0.4415588f,-0.4675598f,-0.4415283f,-0.4415588f,-0.4545593f,-0.4545288f,-0.4285889f,-0.4285583f,-0.4285889f,-0.4025879f,-0.4155884f,-0.4025879f,-0.3766479f,-0.3765869f,-0.3636475f,-0.3636475f,-0.3636475f,-0.3376465f,-0.3246765f,-0.3246765f,-0.311676f,-0.2987366f,-0.2986755f,-0.2857056f,-0.2727661f,-0.285675f,-0.2597656f,-0.2597351f,-0.2467346f,-0.2338257f,-0.2336731f,-0.2208557f,-0.2207336f,-0.1948547f,-0.2077637f,-0.1948242f,-0.1947632f,-0.1948547f,-0.1947632f,-0.1818848f,-0.1817627f,-0.1818542f,-0.1817932f,-0.1818237f,-0.1818237f,-0.1817932f,-0.1558838f,-0.1817932f,-0.1558838f,-0.1557922f,-0.1558838f,-0.1687622f,-0.1559448f,-0.1557617f,-0.1429138f,-0.1688232f,-0.1558228f,-0.1558533f,-0.1558533f,-0.1298218f,-0.1429443f,-0.1297913f,-0.1299438f,-0.1297913f,-0.09094238f,-0.1168823f,-0.09091187f,-0.1038818f,-0.07797241f,-0.1037903f,-0.09103394f,-0.07781982f,-0.07800293f,-0.06488037f,-0.06497192f,-0.0519104f,-0.03900146f,-0.03890991f,-0.03903198f,-0.03887939f,-0.03903198f,-0.03887939f,-0.02606201f,-0.02590942f,-0.02600098f,-0.02597046f,-0.03894043f,-0.01303101f,-0.02593994f,-0.02597046f,-0.03900146f,-0.02590942f,-0.02606201f,-0.03887939f,-0.03900146f,-0.05194092f,-0.03894043f,-0.03900146f,-0.05187988f,-0.06503296f,-0.07781982f,-0.07800293f,-0.09082031f,-0.09100342f,-0.09082031f,-0.1040039f,-0.09082031f,-0.09094238f,-0.09091187f,-0.09091187f,-0.1038818f,-0.1039429f,-0.1038208f,-0.1039734f,-0.1168213f,-0.1039734f,-0.1168213f,-0.1169128f,-0.1298523f,-0.1039124f,-0.1168823f,-0.1168823f,-0.09091187f,-0.1168823f,-0.1168823f,-0.1038818f,-0.1169128f,-0.1168213f,-0.1039734f,-0.1298218f,-0.1168823f,-0.1039124f,-0.1168518f,-0.1039124f,-0.1169128f,-0.1168518f,-0.1168518f,-0.0909729f,-0.1168213f,-0.1039734f,-0.09082031f,-0.1039734f,-0.1038208f,-0.07803345f,-0.07781982f,-0.0909729f,-0.06488037f,-0.07797241f,-0.07788086f,-0.06500244f,-0.07781982f,-0.07803345f,-0.07781982f,-0.07800293f,-0.09082031f,-0.07803345f,-0.09082031f,-0.07797241f,-0.07788086f,-0.09094238f,-0.09088135f,-0.0909729f,-0.09085083f,-0.1168823f,-0.1169434f,-0.1167603f,-0.1170654f,-0.1426697f,-0.1430054f,-0.1427917f,-0.1428223f,-0.1559143f,-0.1557922f,-0.1688538f,-0.1558533f,-0.1817627f,-0.1688843f,-0.1947937f,-0.1947937f,-0.2078247f,-0.2077332f,-0.1948547f,-0.2077942f,-0.2077332f,-0.1948853f,-0.1947327f,-0.2078552f,-0.2077637f,-0.1948242f,-0.1947632f,-0.1948242f,-0.1948242f,-0.1947632f,-0.2078552f,-0.1947327f,-0.1818848f,-0.1817627f,-0.1688843f,-0.1557922f,-0.1558838f,-0.1428528f,-0.1298218f,-0.1169739f,-0.1038208f,-0.09094238f,-0.1039124f,-0.07788086f,-0.07794189f,-0.05194092f,-0.03897095f,-0.0519104f,-0.01309204f,-0.02581787f,-0.0001525879f};
	readonly Color[] graphKeys = new[] {new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,1.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f), new Color(1.0f,1.0f,1.0f,0.0f)};

	AudioClip audioClip;
	AudioSource audioSource;

	Texture2D graphTexture;
	int graphHeight = 100;

	int[,] textOverlayMask;

	float[,] dtHistory;
	int[] gcHistory;
	int i,j,x,y;
	float val;
	Color color;
	Color32 color32;
	float maxFrame = 0.0f;
	float actualFPS = 0.0f;
	float yMulti;
	float beforeRender;
	float[] fpsVals = new float[3];
	float x1;
	float x2;
	float y1;
	float y2;
	float xOff;
	float yOff;
	int[] lineY = new int[]{25, 50, 99};
	int[] lineY2 = new int[]{21, 46, 91};
	int[] keyOffX = new int[]{61,34,1};
	string[] splitMb;
	int first;
	int second;
	float lowestDt = 10000.0f;
	float highestDt;
	float totalDt;
	int totalFrames = 0;
	float totalGPUTime = 0.0f;
	float totalCPUTime = 0.0f;
	float totalOtherTime = 0.0f;
	float totalTimeElapsed = 0.0f;
	float totalSeconds;
	float renderSeconds;
	float lateSeconds;
	float dt;
	int frameCount;
	int frameIter = 1;
	float eTotalSeconds;
	int lastCollectionCount = -1;
	float mem;

	static Color[] fpsColors;
	static Color[] fpsColorsTo;
	
	Color lineColor = new Color(1.0f, 1.0f, 1.0f, 0.25f);
	Color darkenedBack = new Color(0.0f,0.0f,0.0f,0.5f);
	Color darkenedBackWhole = new Color(0.0f,0.0f,0.0f,0.25f);

	Color32[] colorsWrite;

	Rect graphSizeGUI;

	System.Diagnostics.Stopwatch stopWatch;
	float lastElapsed;
	float fps;
	int graphSizeMin;
	enum FPSGraphViewMode{
		graphing,
		totalperformance,
		assetbreakdown
	}
	FPSGraphViewMode viewMode = FPSGraphViewMode.graphing;

	void Awake(){
		if(gameObject.GetComponent<Camera>()==null)
			Debug.LogWarning("FPS Graph needs to be attached to a Camera object");

		CreateLineMaterial();

		fpsColors = new[] { RenderColor, CpuColor, OtherColor };
		fpsColorsTo = new[] {fpsColors[0]*0.7f, fpsColors[1]*0.7f, fpsColors[2]*0.7f};
	}

	IEnumerator Start () {
		graphSizeMin = frameHistoryLength > 95 ? frameHistoryLength : 95;

		textOverlayMask = new int[graphHeight, graphSizeMin];
		reset();

		graphTexture = new Texture2D( graphSizeMin, 7, TextureFormat.ARGB32, false, false);
		colorsWrite = new Color32[ graphTexture.width * 7 ];
		graphTexture.filterMode = FilterMode.Point;
		graphSizeGUI = new Rect( 0f, 0f, graphTexture.width * graphMultiply, graphTexture.height * graphMultiply );

		addFPSAt(14,23);
		addFPSAt(14,48);
		addFPSAt(14,93);
		if(showFPSNumber){
			addFPSAt(14,0);
		}

		for (int x = 0; x < graphTexture.width; ++x) {
			for (int y= 0; y < 7; ++y) {
				if(x < 95 && y < 5){
					color = graphKeys[ y*95 + x ];
				}else{
					color.a = 0.0f;
				}
				graphTexture.SetPixel(x, y, color);
				colorsWrite[ (y) * graphTexture.width + x ] = color;
			}
		}
		graphTexture.Apply();

	  	if(audioFeedback)
	    	initAudio();

	    while (true) {
	        yield return new WaitForEndOfFrame();

	        eTotalSeconds = (float)stopWatch.Elapsed.TotalSeconds;
			dt = eTotalSeconds - lastElapsed;
	    }
	}

	void reset(){
		dtHistory = new float[3, frameHistoryLength];
		gcHistory = new int[frameHistoryLength];

	    stopWatch = new System.Diagnostics.Stopwatch();
	    stopWatch.Start();

	    lowestDt = 10000.0f;
		highestDt = 0f;
		totalDt = 0f;
		totalFrames = 0;
		totalGPUTime = 0f;
		totalCPUTime = 0f;
		totalOtherTime = 0f;
		totalTimeElapsed = 0f;
		frameIter = 0;
		frameCount = 1;
	}

	public void initAudio(){
		audioClip = AudioClip.Create("FPS-BNote", bNote.Length, 1, 44100, false, false);
		audioClip.SetData(bNote, 0);

		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.loop = true;
		audioSource.clip = audioClip;
	}

	int xExtern;
	int yExtern;
	int startAt;
	int yOffset;
	int xLength;

	void addFPSAt( int startX, int startY){
		yExtern = startY;
		for(int y=0; y < 4; y++){
			xExtern = startX;
			yOffset = y*11;
			for(int x=0; x < 11; x++){
				textOverlayMask[yExtern, xExtern] = fpsBits[ yOffset + x ];
				xExtern++;
			}

			yExtern++;
		}
	}

	int k,z;
	void addNumberAt( int startX, int startY, int num, bool isLeading ){
		if(isLeading && num==0)
			num = -1;
		startAt = num * 4;
		xLength = startAt + 3;

		yExtern = startY;
		for(z=0; z < 5; z++){
			xExtern = startX;
			yOffset = z*39;
			for(k=startAt; k < xLength; k++){
				//textOverlayMask[yExtern, xExtern] = num==-1 ? 0 : numberBits[ yOffset + x ];
				if(num!=-1 && numberBits[ yOffset + k ] == 1){
					x1 = xExtern * graphMultiply + xOff;
					y1 = yExtern * graphMultiply + yOff;
					GL.Vertex3(x1,y1,0);
					GL.Vertex3(x1,y1+1* graphMultiply,0);
					GL.Vertex3(x1+1* graphMultiply,y1+1* graphMultiply,0);
					GL.Vertex3(x1+1* graphMultiply,y1,0);
				}
				xExtern++;
			}

			yExtern++;
		}
	}

	void addPeriodAt( int startX, int startY){
	    x1 = startX*graphMultiply + xOff;
	    x2 = (startX+1)*graphMultiply + xOff;
	    y1 = startY*graphMultiply + yOff;
	    y2 = (startY-1)*graphMultiply + yOff;
	    GL.Vertex3(x1,y1,0);
	    GL.Vertex3(x1,y2,0);
	    GL.Vertex3(x2,y2,0);
	    GL.Vertex3(x2,y1,0);
	}

	void Update () {
		if(viewMode == FPSGraphViewMode.graphing){
			lastElapsed = (float)stopWatch.Elapsed.TotalSeconds;
			if(frameCount>4){
				//Debug.Log("Update seconds:"+stopWatch.Elapsed.TotalSeconds);
				//Debug.Log("Update lastElapsed:"+lastElapsed);
				
			    dtHistory[0, frameIter] = dt;

			    if(dt<lowestDt){
					lowestDt = dt;
				}else if(dt>highestDt){
					highestDt = dt;
				}

				if(frameIter%10==0){
					actualFPS = dt;
				}
				totalGPUTime += dtHistory[0, frameIter]-dtHistory[1, frameIter];
				totalCPUTime += dtHistory[1, frameIter]-dtHistory[2, frameIter];
				totalOtherTime += dtHistory[2, frameIter];

				if(lastCollectionCount!=System.GC.CollectionCount(0) ){
					gcHistory[frameIter] = 1;
					lastCollectionCount = System.GC.CollectionCount(0);
				}

				totalDt += dt;
				totalFrames++;

				frameIter++;

				if(audioFeedback){
					if(audioClip==null)
					initAudio();

					if(audioSource.isPlaying==false)
					audioSource.Play();
				}else if(audioSource && audioSource.isPlaying){
					audioSource.Stop();
				}

				if(audioClip){
					audioSource.pitch = Mathf.Clamp( dt * 90.0f - 0.7f, 0.1f, 50.0f );
					audioSource.volume = audioFeedbackVolume;
				}
				//Debug.Log("audioSource.pitch:"+audioSource.pitch);

				if(frameIter>=frameHistoryLength)
					frameIter = 0;

				beforeRender = (float)stopWatch.Elapsed.TotalSeconds;
			}
			frameCount++;
		}

		//Debug.Log("yMulti:"+yMulti + " maxFrame:"+maxFrame);
	}

	void LateUpdate(){
		//Debug.Log("LateUpdate seconds:"+stopWatch.Elapsed.TotalSeconds);

		eTotalSeconds = (float)stopWatch.Elapsed.TotalSeconds;
		dt = (eTotalSeconds - beforeRender);
		//Debug.Log("OnPostRender time:"+dt);

		dtHistory[2, frameIter] = dt;

		beforeRender = eTotalSeconds;
	}

	void OnPostRender(){
		GL.PushMatrix();
		mat.SetPass(0);
		GL.LoadPixelMatrix();
		GL.Begin(GL.QUADS);

		if(viewMode == FPSGraphViewMode.graphing){
			xOff = graphPosition.x*(w - frameHistoryLength*graphMultiply);
			yOff = h - 100 * graphMultiply - graphPosition.y*(h-graphMultiply*107);

			// Shadow for whole graph
			GL.Color( darkenedBackWhole );
			GL.Vertex3(xOff, yOff-8*graphMultiply,0);
			GL.Vertex3(xOff,100 * graphMultiply + yOff,0);
			GL.Vertex3(graphSizeMin * graphMultiply + xOff,100.0f* graphMultiply + yOff,0);
			GL.Vertex3(graphSizeMin * graphMultiply + xOff,yOff-8*graphMultiply,0);
		    
		    maxFrame = 0.0f;
		    for (x = 0; x < frameHistoryLength; ++x) {
				totalSeconds = dtHistory[ 0, x ];

				if(totalSeconds>maxFrame)
					maxFrame = totalSeconds;
				totalSeconds *= yMulti;
				fpsVals[0] = totalSeconds;

				renderSeconds = dtHistory[ 1, x ];
				renderSeconds *= yMulti;
				fpsVals[1] = renderSeconds;

				lateSeconds = dtHistory[ 2, x ];
				lateSeconds *= yMulti;
				fpsVals[2] = lateSeconds;

				i = x - frameIter - 1;
				if(i<0)
					i = frameHistoryLength + i;
			
				x1 = i * graphMultiply + xOff;
				x2 = (i+1) * graphMultiply + xOff;
				
				for(j = 0; j < fpsVals.Length; j++){
					y1 = j < fpsVals.Length - 1 ? fpsVals[j+1] * graphMultiply + yOff : yOff;
					y2 = fpsVals[j] * graphMultiply + yOff;

					GL.Color(fpsColorsTo[j]);
					GL.Vertex3(x1,y1,0);
					GL.Vertex3(x2,y1,0);
					GL.Color(fpsColors[j]);
					GL.Vertex3(x2,y2,0);
					GL.Vertex3(x1,y2,0);
				}

				// Garbage Collections
				if(gcHistory[x]==1){
					y1 = -0*graphMultiply+yOff;
					y2 = -2*graphMultiply+yOff;
					GL.Color(Color.red);
					GL.Vertex3(x1,y1,0);
					GL.Vertex3(x2,y1,0);
					GL.Vertex3(x2,y2,0);
					GL.Vertex3(x1,y2,0);
				}

		      //Debug.Log("x:"+(x-frameIter));
		    }

		  	// Round to nearest relevant FPS
		  	if(useMinFPS==false){
			   	if(maxFrame < 1.0f/120.0f){
			   		maxFrame = 1.0f/120.0f;
			   	}else if(maxFrame < 1.0f/60.0f){
			   		maxFrame = 1.0f/60.0f;
			   	}else if(maxFrame < 1.0f/30.0f){
			   		maxFrame = 1.0f/30.0f;
			   	}else if(maxFrame < 1.0f/15.0f){
			   		maxFrame = 1.0f/15.0f;
			   	}else if(maxFrame < 1.0f/10.0f){
			   		maxFrame = 1.0f/10.0f;
			   	}else if(maxFrame < 1.0f/5.0f){
			   		maxFrame = 1.0f/5.0f;
			   	}

			    yMulti = graphHeight / maxFrame;
			}else{
				maxFrame = 1.0f/minFPS;
				yMulti = graphHeight / maxFrame;
			}

		    
		    // Add Horiz Lines
			GL.Color( lineColor );
			x1 = 28 * graphMultiply + xOff;
			x2 = graphSizeMin*graphMultiply + xOff;
			for(i = 0; i < lineY.Length; i++){
				y1 = lineY[i] * graphMultiply + yOff;
				y2 = (lineY[i]+1)* graphMultiply + yOff;
				GL.Vertex3(x1,y1,0);
				GL.Vertex3(x1,y2,0);
				GL.Vertex3(x2,y2,0);
				GL.Vertex3(x2,y1,0);
			}

			// Add FPS Shadows
			GL.Color( darkenedBack );
			x2 = 27 * graphMultiply + xOff;
			for(i = 0; i < lineY.Length; i++){
				y1 = lineY2[i] * graphMultiply + yOff;
				y2 = (lineY2[i]+9)* graphMultiply + yOff;
				GL.Vertex3(xOff, y1,0);
				GL.Vertex3(xOff, y2,0);
				GL.Vertex3(x2, y2,0);
				GL.Vertex3(x2, y1,0);
			}

			// Add Key Boxes
		    for(i = 0; i < keyOffX.Length; i++){
		        x1 = keyOffX[i]*graphMultiply + xOff + 1*graphMultiply;
		        x2 = (keyOffX[i]+4)*graphMultiply + xOff + 1*graphMultiply;
		        y1 = (5)*graphMultiply + yOff - 9*graphMultiply;
		        y2 = (1)*graphMultiply + yOff - 9*graphMultiply;
		        GL.Color( fpsColorsTo[i] );
		        GL.Vertex3(x1,y1,0);
		        GL.Vertex3(x1,y2,0);
		        GL.Vertex3(x2,y2,0);
		        GL.Vertex3(x2,y1,0);
		    }

		    for(i = 0; i < keyOffX.Length; i++){
		        x1 = keyOffX[i]*graphMultiply + xOff;
		        x2 = (keyOffX[i]+4)*graphMultiply + xOff;
		        y1 = (5)*graphMultiply + yOff - 8*graphMultiply;
		        y2 = (1)*graphMultiply + yOff - 8*graphMultiply;
		        GL.Color( fpsColors[i] );
		        GL.Vertex3(x1,y1,0);
		        GL.Vertex3(x1,y2,0);
		        GL.Vertex3(x2,y2,0);
		        GL.Vertex3(x2,y1,0);
		    }

		    GL.Color(Color.white);
		    for (x = 0; x < graphTexture.width; ++x) {
				for (y = 0; y < graphHeight; ++y) {
					// Draw Text
					if(textOverlayMask[y,x] == 1){
						x1 = x*graphMultiply + xOff;
						x2 = x*graphMultiply + 1*graphMultiply + xOff;
						y1 = y*graphMultiply + yOff;
						y2 = y*graphMultiply + 1*graphMultiply + yOff;
						GL.Vertex3(x1,y1,0);
						GL.Vertex3(x1,y2,0);
						GL.Vertex3(x2,y2,0);
						GL.Vertex3(x2,y1,0);
					}
				}
		    }

		    // Draw Mb
		    for (x = 0; x < 9; ++x) {
		        for (y = 0; y < 4; ++y) {
		            if(mbBits[y*9 + x]==1){
		                x1 = x*graphMultiply + xOff + 111*graphMultiply;
		                x2 = x*graphMultiply + 1*graphMultiply + xOff + 111*graphMultiply;
		                y1 = y*graphMultiply + yOff + -7*graphMultiply;
		                y2 = y*graphMultiply + 1*graphMultiply + yOff + -7*graphMultiply;
		                GL.Vertex3(x1,y1,0);
		                GL.Vertex3(x1,y2,0);
		                GL.Vertex3(x2,y2,0);
		                GL.Vertex3(x2,y1,0);
		            }
		        }
		    }

		    if(maxFrame>0){
				fps = Mathf.Round(1.0f/maxFrame);
				
				if(showFPSNumber && actualFPS>0.0f){
					float roundedFPS = Mathf.Round(1.0f/actualFPS);

					addNumberAt( 1, 0, (int)((roundedFPS / 100)%10), true );
					addNumberAt( 5, 0, (int)((roundedFPS / 10.0)%10), false );
					addNumberAt( 9, 0, (int)(roundedFPS % 10), false );
				}

				addNumberAt( 1, 93, (int)((fps / 100)%10), true );
				addNumberAt( 5, 93, (int)((fps / 10.0)%10), true );
				addNumberAt( 9, 93, (int)(fps % 10), false );

				fps *= 2;
				addNumberAt( 1, 48, (int)((fps / 100)%10), true );
				addNumberAt( 5, 48, (int)((fps / 10)%10), true );
				addNumberAt( 9, 48, (int)(fps % 10), false );

				fps *= 1.5f;
				addNumberAt( 1, 23, (int)((fps / 100)%10), true );
				addNumberAt( 5, 23, (int)((fps / 10)%10), true );
				addNumberAt( 9, 23, (int)(fps % 10), false );
				
				mem = ( System.GC.GetTotalMemory(false) ) / 1000000.0f;

				if(mem<1.0){
		            splitMb = mem.ToString("F2").Split("."[0]);

		            if(splitMb[1][0]=="0"[0]){
		                first = 0;
		                second = int.Parse( splitMb[1] );
		            }else{
		                first = int.Parse( splitMb[1] );
		                second = first%10;
		                first = (first/10)%10;
		            }
		            
		            addPeriodAt( 100, -6);
		            addNumberAt( 102, -7, first, false );
		            addNumberAt( 106, -7, second, false );
		        }else if(mem<100.0f){
		        	splitMb = mem.ToString("F1").Split("."[0]);
		            first = int.Parse( splitMb[0] );

		            if(first>=10)
		                addNumberAt( 96, -7, first%100/10, false );
		            addNumberAt( 100, -7, first%10, false );
		            addPeriodAt( 104, -6);
		            addNumberAt( 106, -7, int.Parse( splitMb[1] ), false );
		        }else{
		        	first = (int)mem;

		        	addNumberAt( 96, -7, (int)(first/100), false );
		        	addNumberAt( 100, -7, first%100/10, false );
		        	addNumberAt( 104, -7, first%10, false );
		        }
		    }
	    }else{
	    	if(circleGraphLabels==null)
	    		circleGraphLabels = new Vector2[3];
	    	// draw background
	    	Rect sRect = new Rect(w*0.05f,h*0.05f,w*0.9f,h*0.9f);
	    	GL.Color(new Color(0f,0f,0f,0.8f));
	    	GL.Vertex3(sRect.x,sRect.y,0f);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y,0f);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y+sRect.height,0f);
	    	GL.Vertex3(sRect.x,sRect.y+sRect.height,0f);

	    	if(viewMode == FPSGraphViewMode.totalperformance){
	    		// circle graph
		    	float totalTimes = totalCPUTime + totalGPUTime + totalOtherTime;
		    	float[] ratiosWidth = new float[]{totalGPUTime/totalTimes,totalCPUTime/totalTimes,totalOtherTime/totalTimes};
			    float[] ratios = new float[]{ratiosWidth[0],ratiosWidth[0]+ratiosWidth[1],1.0f};
			    float x;
			    float y;
			    float length = w*0.15f;
			    float angle = 0.0f;
			    float angle_stepsize = Mathf.PI/120.0f;
			    Vector2 center = new Vector2(w*0.7f,h*0.5f);
			    int colorIter = 0;
			    float colorIterRatio = 0.0f;

			    // Labels
			    for(colorIter = 0; colorIter < 3; colorIter++){
			    	float centerAngle = (ratios[colorIter] - ratiosWidth[colorIter] * 0.5f) * (2.0f * Mathf.PI);
			    	// Debug.Log("colorIter:"+colorIter+ " x:"+length * 0.5f * Mathf.Cos(centerAngle));
			    	x = length * 0.3f * Mathf.Cos(centerAngle);
			    	x = x < 0 ? x + x : x;
		    		x = center.x + x;
			        y = center.y + length * 0.5f * Mathf.Sin(centerAngle) + 0.02f*h;
		    		circleGraphLabels[colorIter] = new Vector2(x,Screen.height-y);
			    }

			    colorIter = 0;
			    while (angle < 2.0f * Mathf.PI){
			    	float ratio = angle / (2.0f * Mathf.PI);
			    	if(ratio > ratios[colorIter]){
			    		colorIter++;
			    		colorIterRatio = 0.0f;
			    	}else{
			    		colorIterRatio += angle_stepsize / (2.0f*Mathf.PI);
			    	}
			    	Color diff = (fpsColors[colorIter]-fpsColors[colorIter]*0.4f);
			    	float colorRatio = colorIterRatio/ratiosWidth[colorIter];
			    	GL.Color(fpsColors[colorIter]*0.85f+diff*colorRatio);
			    	
			        x = center.x + length * Mathf.Cos(angle);
			        y = center.y + length * Mathf.Sin(angle);
			        angle += angle_stepsize;

			        GL.Vertex3(center.x,center.y,0f);
			        GL.Vertex3(x,y,0f);
			        x = center.x + length * Mathf.Cos(angle);
			        y = center.y + length * Mathf.Sin(angle);
			    	GL.Vertex3(x,y,0f);
					GL.Vertex3(center.x,center.y,0f);
			    }
			}

		    // Dismiss Box
		    sRect = new Rect(w*0.375f,h*0.08f,w*0.25f,h*0.11f);
		   	GL.Color(fpsColorsTo[1]);
		    GL.Vertex3(sRect.x,sRect.y,0f);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y,0f);
	    	
	    	GL.Color(fpsColors[1]);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y+sRect.height,0f);
	    	GL.Vertex3(sRect.x,sRect.y+sRect.height,0f);

	    	// Top Tabs
	    	float xStart = viewMode == FPSGraphViewMode.assetbreakdown ? w*0.05f : 0.5f*w;
	    	sRect = new Rect(xStart,h*0.84f,w*0.45f,h*0.11f);
		   	GL.Color(fpsColorsTo[1]);
		    GL.Vertex3(sRect.x,sRect.y,0f);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y,0f);
	    	
	    	GL.Color(fpsColors[1]);
	    	GL.Vertex3(sRect.x+sRect.width,sRect.y+sRect.height,0f);
	    	GL.Vertex3(sRect.x,sRect.y+sRect.height,0f);
	    }

	    GL.End();
	    GL.PopMatrix();

		dt = ((float)stopWatch.Elapsed.TotalSeconds - beforeRender);
		//Debug.Log("OnPostRender time:"+dt);

		dtHistory[1, frameIter] = dt;

		eTotalSeconds = (float)stopWatch.Elapsed.TotalSeconds;

		dt = (eTotalSeconds - lastElapsed);
		//Debug.Log("Update time:"+dt*1000 + " Time.delta:"+Time.deltaTime*1000);
	}

	bool hasFormated;
	Rect wRect;
	GUIStyle backupLabel;
	GUIStyle backupButton;
	GUIStyle h1;
	GUIStyle h2;
	GUIStyle h3;
	GUIStyle guiButton;
	GUIStyle graphTitles;
	Vector2[] circleGraphLabels;

	void format(){
		if(hasFormated==false){
			hasFormated = true;

			h1 = GUI.skin.GetStyle( "Label" );
			backupLabel = new GUIStyle(h1);
			backupButton = new GUIStyle( GUI.skin.GetStyle("Button") );
			h1.alignment = TextAnchor.UpperLeft;
			h1.fontSize = (int)(Screen.height*0.08f);
			h2 = new GUIStyle( h1 );
			h2.fontSize = (int)(Screen.height*0.05f);
			h3 = new GUIStyle( h1 );
			h3.fontSize = (int)(Screen.height*0.037f);
			graphTitles = new GUIStyle( h1 );
			graphTitles.fontSize = (int)(Screen.height*0.037f);
			// graphTitles.alignment = TextAnchor.LowerCenter;
			guiButton = new GUIStyle(h1);
			guiButton.normal.background = null;
		}
	}

	float w;
	float h;

	void OnGUI(){
	    //Debug.Log("OnGUI time:"+stopWatch.Elapsed);
	    w = Screen.width;
	    h = Screen.height;
	    
	    if(viewMode != FPSGraphViewMode.graphing){
	    	Time.timeScale = 0.0f;
	    	format();
	    	Color backupColor = GUI.color;
	    	GUI.color = Color.black;

	    	Rect sRect = new Rect(w*0.05f,h*0.05f,w*0.9f,h*0.9f);
	    	GUI.color = Color.black;

	    	GUI.color = Color.white;
	    	GUI.skin.label = h2;
			GUI.Label(new Rect(w*0.1f,h*0.07f,w,h*0.2f), "Performance Results");
			GUI.Label(new Rect(w*0.62f,h*0.07f,w,h*0.2f), "Assets Used");

			if(viewMode == FPSGraphViewMode.totalperformance){
				GUI.skin.label = h2;
				GUI.Label(new Rect(w*0.1f,h*0.2f,w,h*0.2f), "Score:");
				GUI.skin.label = h1;
				GUI.Label(new Rect(w*0.1f,h*0.27f,w,h*0.2f), (totalDt*1000.0f).ToString("n0")+"ms");

				GUI.skin.label = h2;
				GUI.Label(new Rect(w*0.1f,h*0.38f,w,h*0.2f), "Time Elapsed:");
				GUI.skin.label = h1;
				GUI.Label(new Rect(w*0.1f,h*0.43f,w,h*0.2f), totalTimeElapsed.ToString("F1")+"s");

				GUI.skin.label = h3;
				float avgFrameRate = totalDt / totalFrames;
				string[] arr = new string[]{"lowest: "+(1.0f/highestDt).ToString("n0")+"fps","highest: "+(1.0f/lowestDt).ToString("n0")+"fps", "avg: "+(1.0f/avgFrameRate).ToString("n0")+"fps"};
				for(int i = 0; i < arr.Length; i++){
					GUI.Label(new Rect(w*0.1f,h*0.57f + w*0.04f*i,w,h*0.2f), arr[i]);
				}

				GUI.color = Color.black;
				GUI.skin.label = graphTitles;
				arr = new string[]{"Render","CPU","Other"};
				float[] arrW = new float[]{0.12f,0.12f,0.12f};
				float sh = 0.0023f*w;
				for(int i = 0; circleGraphLabels!=null && i<circleGraphLabels.Length; i++){
					GUI.color = Color.black;
					GUI.Label(new Rect(circleGraphLabels[i].x+sh,circleGraphLabels[i].y+sh,w*arrW[i],h*0.047f), arr[i]);
					GUI.color = Color.white;
					GUI.Label(new Rect(circleGraphLabels[i].x,circleGraphLabels[i].y,w*arrW[i],h*0.047f), arr[i]);
				}
			}else{
				GUILayout.BeginArea( new Rect(w*0.08f,h*0.175f,w*0.9f,h*0.7f) );
				GUI.skin.label = h2;
				GUILayout.Label("All: " + Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object)).Length.ToString("n0"));
				GUILayout.Label("Textures: " + Resources.FindObjectsOfTypeAll(typeof(Texture)).Length.ToString("n0"));
				GUILayout.Label("AudioClips: " + Resources.FindObjectsOfTypeAll(typeof(AudioClip)).Length.ToString("n0"));
				GUILayout.Label("Meshes: " + Resources.FindObjectsOfTypeAll(typeof(Mesh)).Length.ToString("n0"));
				GUILayout.Label("Materials: " + Resources.FindObjectsOfTypeAll(typeof(Material)).Length.ToString("n0"));
				GUILayout.Label("GameObjects: " + Resources.FindObjectsOfTypeAll(typeof(GameObject)).Length.ToString("n0"));
				GUILayout.Label("Components: " + Resources.FindObjectsOfTypeAll(typeof(Component)).Length.ToString("n0"));
				GUILayout.EndArea();
			}

			GUI.skin.button = guiButton;
			if(GUI.Button(new Rect(w*0.05f,h*0.05f,w*0.45f,h*0.15f),"")){
				viewMode = FPSGraphViewMode.totalperformance;
			}
			if(GUI.Button(new Rect(w*0.5f,h*0.05f,w*0.45f,h*0.15f),"")){
				viewMode = FPSGraphViewMode.assetbreakdown;
			}
			if(GUI.Button(new Rect(w*0.3f,h*0.8f,w*0.4f,h*0.15f),"")){
				reset();
				viewMode = FPSGraphViewMode.graphing;
				Time.timeScale = 1.0f;
			}
			sRect = new Rect(w*0.435f,h*0.83f,w*0.25f,h*0.11f);
			GUI.skin.label = h2;
			GUI.Label(sRect, "Dismiss");

			GUI.skin.label = backupLabel;
			GUI.skin.button = backupButton;
			GUI.color = backupColor;
	    }else{
	    	if(Time.frameCount>4)
				GUI.DrawTexture( new Rect(graphPosition.x*(w-graphMultiply*frameHistoryLength), graphPosition.y*(h-graphMultiply*107) + 100*graphMultiply, graphSizeGUI.width, graphSizeGUI.height), graphTexture );
		}

	    if(showPerformanceOnClick && didPressOnGraph() && highestDt>0.0f)
	    	showPerformance();
	}

	public void showPerformance(){
		if(viewMode!=FPSGraphViewMode.totalperformance){
			totalTimeElapsed = Time.time;
			viewMode = FPSGraphViewMode.totalperformance;
			if(audioSource)
		    	audioSource.Stop();
		}
	}

	public bool didPressOnGraph(){
		if(Input.touchCount>0||Input.GetMouseButtonDown(0)){
			Rect graphRect = new Rect(graphPosition.x*(w-graphMultiply*frameHistoryLength),graphPosition.y*(h-graphMultiply*107),graphSizeGUI.width,107*graphMultiply);
			if(Input.touchCount>0){
				for(int i=0; i < Input.touchCount; i++){
					if(Input.touches[i].phase == TouchPhase.Ended && checkWithinRect( Input.touches[i].position, graphRect ))
						return true;
				}
			}else if(Input.GetMouseButtonDown(0) && checkWithinRect( Input.mousePosition, graphRect )){
				return true;
			}
		}

		return false;
	}

	public static bool checkWithinRect(Vector2 vec2, Rect rect){
		vec2.y = Screen.height-vec2.y;
		return (vec2.x > rect.x && vec2.x < rect.x + rect.width && vec2.y > rect.y && vec2.y < rect.y + rect.height);
	}

}