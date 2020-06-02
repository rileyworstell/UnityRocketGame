using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
  [SerializeField] float rcsThrust = 100f;
  [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
      rigidBody = GetComponent<Rigidbody>();

      audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    { if (state == State.Alive) {


      Thrust();
      Rotate();
    }
    }

void OnCollisionEnter(Collision collision) {
  if (state != State.Alive) {
    return;
  }
  switch (collision.gameObject.tag) {
    case "Friendly":
      print("OK");
      break;
    case "Finish":
    state = State.Transcending;
    Invoke("LoadNextLevel", 1f);

    break;
    default:
    state = State.Dying;
    Invoke("LoadFirstLevel", 1f);
    // destroy ship
        break;
    }
  }

private void LoadNextLevel() {
  SceneManager.LoadScene(1);
}
private void LoadFirstLevel() {
  SceneManager.LoadScene(0);
}

    private void Rotate() {
      rigidBody.freezeRotation = true; // take manual rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;
      if (Input.GetKey(KeyCode.A)) {
        transform.Rotate(Vector3.forward * rotationThisFrame);
      }
      else if (Input.GetKey(KeyCode.D)) {
        transform.Rotate(-Vector3.forward * rotationThisFrame);
      }
      rigidBody.freezeRotation = false; //
    }


private void Thrust() {
  if (Input.GetKey(KeyCode.Space)) { // can thrust while rotating
    //
      rigidBody.AddRelativeForce(Vector3.up * mainThrust);
      if (!audioSource.isPlaying) {
        audioSource.Play();
      }

  }
  else {
    audioSource.Stop();
  }
}


}
