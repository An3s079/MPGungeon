using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MPGungeon
{
    public static class Tools
    {
		public static void SetScale(this Transform transform, float scale)
		{
			transform.localScale = Vector3.one * scale;
		}

		public static void SetScaleX(this Transform transform, float scaleX)
		{
			transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
		}

		public static void SetScaleY(this Transform transform, float scaleY)
		{
			transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
		}

		public static void SetScaleZ(this Transform transform, float scaleZ)
		{
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, scaleZ);
		}
	}
}
