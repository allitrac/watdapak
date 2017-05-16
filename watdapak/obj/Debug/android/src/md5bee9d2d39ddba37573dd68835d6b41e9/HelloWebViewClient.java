package md5bee9d2d39ddba37573dd68835d6b41e9;


public class HelloWebViewClient
	extends android.webkit.WebViewClient
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_shouldOverrideUrlLoading:(Landroid/webkit/WebView;Ljava/lang/String;)Z:GetShouldOverrideUrlLoading_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"n_onPageFinished:(Landroid/webkit/WebView;Ljava/lang/String;)V:GetOnPageFinished_Landroid_webkit_WebView_Ljava_lang_String_Handler\n" +
			"";
		mono.android.Runtime.register ("watdapak.HelloWebViewClient, watdapak, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", HelloWebViewClient.class, __md_methods);
	}


	public HelloWebViewClient () throws java.lang.Throwable
	{
		super ();
		if (getClass () == HelloWebViewClient.class)
			mono.android.TypeManager.Activate ("watdapak.HelloWebViewClient, watdapak, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public HelloWebViewClient (md5bee9d2d39ddba37573dd68835d6b41e9.MainActivity p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == HelloWebViewClient.class)
			mono.android.TypeManager.Activate ("watdapak.HelloWebViewClient, watdapak, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "watdapak.MainActivity, watdapak, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public boolean shouldOverrideUrlLoading (android.webkit.WebView p0, java.lang.String p1)
	{
		return n_shouldOverrideUrlLoading (p0, p1);
	}

	private native boolean n_shouldOverrideUrlLoading (android.webkit.WebView p0, java.lang.String p1);


	public void onPageFinished (android.webkit.WebView p0, java.lang.String p1)
	{
		n_onPageFinished (p0, p1);
	}

	private native void n_onPageFinished (android.webkit.WebView p0, java.lang.String p1);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
