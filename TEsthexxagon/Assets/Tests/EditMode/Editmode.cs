using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class Editmode
    {

        [Test]
        public void NoClick()
        {

            var click = new GameObject().AddComponent<ClickCoord>();
            Assert.IsFalse(click.click);
        }



        [Test]
        public void CheckTileMap()
        {
            var tilemap = new GameObject().AddComponent<ClickCoord>();
            bool test = true;

            if (test)
            {
                Assert.IsNull(tilemap.myTileMap);
                Assert.Pass("Tilemap is null");
            }
            else
            {
                Assert.IsNotNull(tilemap.myTileMap);
                Assert.Fail("Tilemap Is already charged");
            }

        }
    }
}
