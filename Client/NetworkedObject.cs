using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace MpGungeon.Client
{
	class NetworkedObject : MonoBehaviour
	{
		private float _t;
		public int ID;
		Vector3 prevPos;
		string prevClip;
		void Update()
		{
			if (gameObject != null)
			{
				if (gameObject.transform.position != prevPos)
				{
					float dur = 1f / Client.instance.TicksPerSecond;
					_t += Time.deltaTime;
					int cnt = 3;
					while (_t > dur && cnt > 0)
					{
						_t -= dur;
						cnt--;
						ClientSend.SendObjectPos(gameObject.transform.position, ID);
						prevPos = gameObject.transform.position;
					}
				}
			}
			
			var sprite = gameObject.GetComponent<tk2dSpriteAnimator>();
			if (gameObject.GetComponent<PlayerController>() != null)
			{
				sprite = gameObject.GetComponent<PlayerController>().spriteAnimator;
			}
				if (sprite != null)
				{
					if (prevClip != sprite.CurrentClip.name)
					{
						ClientSend.SendSpriteAnim(sprite.CurrentClip.name, ID);
						prevClip = sprite.CurrentClip.name;
					}
				}
		}
	}
}
