/******************************************************************************\
* This is a Singleton class that emulates the axis input that nmost games have *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;

/// <summary>

/// </summary>
public static class pxsLeapInput 
{	
	private enum HandID : int
	{
		Primary		= 0,
		Secondary	= 1
	};
 	
	//Create a new leap controller object when you create this static class 
	static Leap.Controller 		m_controller	= null;
	static Leap.Frame			m_Frame			= null;
	static Leap.Hand			m_Hand			= null;
	static Leap.Hand			m_Hand2			= null;
	static string Errors 			= "";
	
	// constructor called when the first class or member is referenced.
	static pxsLeapInput()
	{
		try
		{
			//Create a new leap controller object when you create this static class 
			m_controller	= new Leap.Controller();
		}
		catch 
		{
			Errors = Errors + '\r' + '\n'  + "Controller could not be created";
		}
	}

	public static Leap.Frame Frame
	{
		get { return m_Frame; }
	}
	
	public static Leap.Hand Hand
	{
		get { return m_Hand; }
	}
	
	public static void Update() 
	{	
		if( m_controller != null )
		{
			
			Frame lastFrame = m_Frame == null ? Frame.Invalid : m_Frame;
			m_Frame	= m_controller.Frame();
			if (m_Frame != null)
			{
				if (m_Frame.Hands.Count > 0)
				{
					if(m_Frame.Hands.Count < 2) {
						m_Hand = m_Frame.Hands[0]; 
					} else {						
						m_Hand = m_Frame.Hands[0]; 
						m_Hand2 = m_Frame.Hands[1]; 
					}
				}
			}
		}
	} 
	// returns the hand axis scaled from -1 to +1
	public static float[] GetHandAxis(string axisName)
	{ 
		float[] ret = GetHandAxisPrivate(axisName, true);
		return ret;
	}
	
	public static float[] GetHandAxisRaw(string axisName)
	{
		float[] ret = GetHandAxisPrivate(axisName, false);
		return ret;
	}
	private static float[] GetHandAxisPrivate(string axisName, bool scaled)
	{
		// Call Update so you can get the latest frame and hand
		Update();
		float ret = 0.0F;
		float ret2 = 0.0F;

		if (m_Hand != null)
		{
			Vector3 PalmPosition = new Vector3(0,0,0);
			Vector3 PalmNormal = new Vector3(0,0,0);
			Vector3 PalmDirection = new Vector3(0,0,0);

			Vector3 PalmPosition_2 = new Vector3(0,0,0);
			Vector3 PalmNormal_2 = new Vector3(0,0,0);
			Vector3 PalmDirection_2 = new Vector3(0,0,0);

			if (scaled == true)
			{
				PalmPosition = m_Hand.PalmPosition.ToUnityTranslated();
				PalmNormal = m_Hand.PalmNormal.ToUnity();				
				PalmDirection = m_Hand.Direction.ToUnity();
			}
			else
			{
				PalmPosition = m_Hand.PalmPosition.ToUnity();
				PalmNormal = m_Hand.PalmPosition.ToUnity();
				PalmDirection = m_Hand.Direction.ToUnity();
			} 

			if(m_Hand2 != null) {				
				PalmPosition_2 = m_Hand2.PalmPosition.ToUnityTranslated();
				PalmNormal_2 = m_Hand2.PalmNormal.ToUnity();				
				PalmDirection_2 = m_Hand2.Direction.ToUnity();
			}

			switch (axisName)
			{
			case "Horizontal":
				ret = PalmPosition.x ;
				ret2 = PalmPosition_2.x ;
				break;
			case "Vertical":
				ret = PalmPosition.y;
				ret2 = PalmPosition_2.y;
				break;
			case "Depth":
				ret = PalmPosition.z ;
				ret2 = PalmPosition_2.z ;
				break;
			case "RotationX":
				ret = -2 * PalmNormal.x ;
				ret2 = -2 * PalmNormal_2.x ;
				break;
			case "RotationY":
				ret = -2 * PalmNormal.y ;
				ret2 = -2 * PalmNormal_2.y ;
				break;
			case "RotationZ":
				ret = -2 * PalmNormal.z ;
				ret2 = -2 * PalmNormal_2.z ;
				break;
			case "Tilt":
				ret = PalmNormal.z ;
				ret2 = PalmNormal_2.z ;
				break;
			case "HorizontalDirection":
				ret = PalmDirection.x ;
				ret2 = PalmDirection_2.x ;
				break;
			case "VerticalDirection":
				ret = PalmDirection.y ;
				ret2 = PalmDirection_2.y ;
				break;
			default:
				break;
			}

		}
		if (scaled == true && axisName != "Vertical")
		{
			if (ret > 10) {ret = 10;}
			if (ret < -10) {ret = -10;}
			if (ret2 > 10) {ret2 = 10;}
			if (ret2 < -10) {ret2 = -10;}
		}

		return new []{ret, ret2};
	}
	
}
