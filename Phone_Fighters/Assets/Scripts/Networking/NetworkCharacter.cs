using UnityEngine;

public class NetworkCharacter : Photon.MonoBehaviour {

    Vector3 realPosition = Vector3.zero;
    //Quaternion realRotation = Quaternion.identity;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, realPosition, 0.3f);
            //transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, 0.01f);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(anim.GetFloat("Run"));
            stream.SendNext(anim.GetBool("Shoot"));
            stream.SendNext(anim.GetBool("Dash"));
        }
        else
        {
            // Network player, receive data
            realPosition = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
            anim.SetFloat("Run", (float)stream.ReceiveNext());
            anim.SetBool("Dash", (bool)stream.ReceiveNext());
            anim.SetBool("Shoot", (bool)stream.ReceiveNext());
        }
    }
}
