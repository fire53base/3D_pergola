using BzKovSoft.ObjectSlicer;
using BzKovSoft.ObjectSlicer.Samples;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BzKovSoft.ObjectSlicerSamples
{
	public class MySlicer : BzSliceableObjectBase, IBzSliceableNoRepeat
	{
		
		protected override BzSliceTryData PrepareData(Plane plane)
		{
			


			// colliders that will be participating in slicing
			var colliders = gameObject.GetComponentsInChildren<Collider>();

			// return data
			return new BzSliceTryData()
			{
				// componentManager: this class will manage components on sliced objects
				componentManager = new StaticComponentManager(gameObject, plane, colliders),
				plane = plane,
				
			};
		}

		protected override void OnSliceFinished(BzSliceTryResult result)
		{
			
		}

		public void Slice(Plane plane, int slicdId, Action<BzSliceTryResult> callBack)
		{
			throw new NotImplementedException();
		}
	}
}
