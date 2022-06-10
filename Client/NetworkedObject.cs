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
		PlayerController player;
		Position pos;
		bool flipped; 
		void Start()
		{
			player = gameObject.GetComponent<PlayerController>();
			flipped = player.SpriteFlipped;
		}
		void Update()
		{
			if (gameObject != null)
			{
				if(player != null)
				{
					if (pos.GetPixelVector2() != player.specRigidbody.Position.GetPixelVector2())
					{
						ClientSend.SendObjectPosWithRemainder(new Vector2(player.specRigidbody.Position.X, player.specRigidbody.Position.Y), player.specRigidbody.Position.Remainder, ID);
						prevPos = new Vector2(player.specRigidbody.Position.X, player.specRigidbody.Position.Y); 
					}
					if(player.SpriteFlipped != flipped)
					{
						ClientSend.SendFlipped(player.SpriteFlipped, ID);
						flipped = player.SpriteFlipped;
					}
				}
				else if (gameObject.transform.position != prevPos)
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
