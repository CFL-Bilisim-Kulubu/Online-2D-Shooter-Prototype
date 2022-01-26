using Photon.Bolt;
using UnityEngine;

public class Callbacks : GlobalEventListener
{
    [SerializeField] private Color[] TakımRengi;
    [SerializeField] private bool Takımlı = false;
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private GameObject playerPrefab;
    private Senkranizasyon s;
    private Color player;

    [System.Obsolete]
    public override void SceneLoadLocalDone(string scene, IProtocolToken protocolToken)
    {
        if(!Takımlı)
            Spawn(0);
    }
    public void Spawn(int team)
    {
        if (!spawnArea) spawnArea = GameObject.FindGameObjectWithTag("Spawn Area").GetComponent<BoxCollider>();

        Vector3 random = new Vector3(spawnArea.bounds.extents.x * Random.Range(-1, 1),
                                     spawnArea.bounds.extents.y * Random.Range(-1, 1),
                                     spawnArea.bounds.extents.z * Random.Range(-1, 1));

        //Vector3 spawn = spawnPoint.position + new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

        GameObject g = BoltNetwork.Instantiate(playerPrefab, spawnArea.bounds.center + random, spawnArea.transform.rotation);
        Debug.Log("Yeni Oyuncu Spawnladım");

        if (Takımlı)
        {
            s = g.GetComponent<Senkranizasyon>();
            player = s.state.Color;// oyuncu rengini depola

            s.takım = team;
            s.takımlı = true;
            s.changeID(); // takımı ayarla

            s.state.Color = TakımRengi[team];
            VisualisePlayer p = VisualisePlayer.Create();
            p.Send(); //rengi takım rengi yap
        }
    }
    public void renk()
    {
        s.Ayarla(player); //rengimiz takım rengi olunca tekrar değiştirdik
    }
}