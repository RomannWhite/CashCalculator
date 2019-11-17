package md5b52cddee5f89390b5dbfe3b32118fd6e;


public class CustomActivity
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("CashCalculator.Activity.CustomActivity, CashCalculator", CustomActivity.class, __md_methods);
	}


	public CustomActivity ()
	{
		super ();
		if (getClass () == CustomActivity.class)
			mono.android.TypeManager.Activate ("CashCalculator.Activity.CustomActivity, CashCalculator", "", this, new java.lang.Object[] {  });
	}

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
