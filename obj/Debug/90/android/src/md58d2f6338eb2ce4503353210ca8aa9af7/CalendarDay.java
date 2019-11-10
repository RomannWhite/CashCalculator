package md58d2f6338eb2ce4503353210ca8aa9af7;


public class CalendarDay
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("CashCalculator.CalendarDay, CashCalculator", CalendarDay.class, __md_methods);
	}


	public CalendarDay ()
	{
		super ();
		if (getClass () == CalendarDay.class)
			mono.android.TypeManager.Activate ("CashCalculator.CalendarDay, CashCalculator", "", this, new java.lang.Object[] {  });
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
