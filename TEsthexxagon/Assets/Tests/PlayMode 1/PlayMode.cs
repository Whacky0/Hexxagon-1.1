using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayMode
    {

        [UnityTest]
        public IEnumerator chequearVectoresVecinos()
        {
            var casillasVecinas = new GameObject().AddComponent<ClickCoord>();
            casillasVecinas.MouseCoordinateLastPos = new Vector3Int(1, 3, 0);

            casillasVecinas.neighborhoods(casillasVecinas.MouseCoordinateLastPos.x, casillasVecinas.MouseCoordinateLastPos.y);
            Debug.Log("Vector de inicio: " + casillasVecinas.MouseCoordinateLastPos);

            for(int i = 0; i < casillasVecinas.devolverColor.Length; i++)
            {
                Assert.AreEqual(actual: casillasVecinas.devolverColor[i],
                    expected: casillasVecinas.vecinosCercanos[i]);

                Debug.Log("Vectores vecinos: " + casillasVecinas.vecinosCercanos[i]);
            }


            yield return null;
        }

        [UnityTest]
        public IEnumerator borrarVecinos()
        {
            var vecinos = new GameObject().AddComponent<ClickCoord>();
            vecinos.borrarPosicionesVecinos();
            CollectionAssert.AreEqual(actual: vecinos.devolverColor, expected: null);
            yield return null;

        }


       
    }
}
