using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBehavior : MonoBehaviour
{

    SnakeMovement SM;
    
    public AudioSource MusicEatFood;
    public AudioClip SoundEatFood;
    

    // Use this for initialization
    void Start()
    {
        SM = transform.GetComponentInParent<SnakeMovement>();
       
        MusicEatFood.clip = SoundEatFood;
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Also need to check if this is the first snake part
        if (collision.transform.tag == "Box" && transform == SM.BodyParts[0])
        {
            //Reset the parent of the text Mesh
            if (SM.BodyParts.Count > 1 && SM.BodyParts[1] != null)
            {
                SM.PartsAmountTextMesh.transform.parent = SM.BodyParts[1];
                SM.PartsAmountTextMesh.transform.position = SM.BodyParts[1].position +
                    new Vector3(0, 0.5f, 0);
            }
            else if (SM.BodyParts.Count == 1)
            {
                SM.PartsAmountTextMesh.transform.parent = null;
            }

            //Stop the Particles
            SM.SnakeParticle.Stop();

            //Move the Particle system to the collision position
            SM.SnakeParticle.transform.position = collision.contacts[0].point;

            //Play the particles
            SM.SnakeParticle.Play();

            //Destroy the Part of the snake that hit the box
            Destroy(this.gameObject);

            //Add one to the Score
            GameController.SCORE++;

            //Diminish the text of the box
            collision.transform.GetComponent<AutoDestroy>().life -= 1;
            collision.transform.GetComponent<AutoDestroy>().UpdateText();

            //change its box color
            collision.transform.GetComponent<AutoDestroy>().SetBoxColor();

            //Remove it from the body parts list to avoid errors
            SM.BodyParts.Remove(SM.BodyParts[0]);


            SM.BodyParts[0].GetComponent<SpriteRenderer>().sprite = SM.HeadSprite;
        }


        else if (collision.transform.tag == "SimpleBox" && transform == SM.BodyParts[0])
        {
            //Stop the Particles
            SM.SnakeParticle.Stop();

            //Move the Particle system to the collision position
            SM.SnakeParticle.transform.position = collision.contacts[0].point;

            //Play the particles
            SM.SnakeParticle.Play();

            // Reset the parent of the text Mesh
            if (SM.BodyParts.Count > 1 && SM.BodyParts[1] != null)
            {
                SM.PartsAmountTextMesh.transform.parent = SM.BodyParts[1];
                SM.PartsAmountTextMesh.transform.position = SM.BodyParts[1].position +
                    new Vector3(0, 0.5f, 0);
            }
            else if (SM.BodyParts.Count == 1)
            {
                SM.PartsAmountTextMesh.transform.parent = null;
            }
            
            //Destroy the Part of the snake that hit the box
            Destroy(this.gameObject);

            //Add one to the Score
            GameController.SCORE++;

            //Diminish the text of the box
            collision.transform.GetComponent<AutoDestroy>().life -= 1;
            collision.transform.GetComponent<AutoDestroy>().UpdateText();

            //change its box color
            collision.transform.GetComponent<AutoDestroy>().SetBoxColor();

            //Remove it from the body parts list to avoid errors
            SM.BodyParts.Remove(SM.BodyParts[0]);

            SM.BodyParts[0].GetComponent<SpriteRenderer>().sprite = SM.HeadSprite;
        }
        else if (collision.transform.tag == "SimpleBox" && transform != SM.BodyParts[0])
        {
            Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), collision.transform.GetComponent<Collider2D>());
            collision.transform.GetComponent<AutoDestroy>().dontMove = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If we collide with the food
        if (SM.BodyParts.Count > 0)
        {
            if (collision.transform.tag == "Food" && transform == SM.BodyParts[0])
            {
                MusicEatFood.Play();
                //Add a body part, will be changed to the amount of body parts it has to add
                for (int i = 0; i < collision.transform.GetComponent<FoodBehavior>().foodAmount; i++)
                {
                    SM.AddBodyPart();
                }

                //Destroy the food
                Destroy(collision.transform.gameObject);
            }
        }

    }


}
