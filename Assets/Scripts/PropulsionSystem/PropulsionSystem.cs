using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.PlayerLoop;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEngine.InputSystem.Composites;

public class PropulsionSystem : MonoBehaviour, IF_PropulsionSystem
{
  public float m_maxAcceleration;
  public float m_maxDeacceleration;
  public float m_acceleration;

  Rigidbody m_rigidBody;
  public Gamepad m_Gamepad;

  void Awake()
  {
    m_rigidBody = GetComponent<Rigidbody>() != null ? GetComponent<Rigidbody>() : gameObject.AddComponent<Rigidbody>();
    m_Gamepad = m_Gamepad == null ? Gamepad.current : m_Gamepad;
  }
  
  /// <summary>
  /// Changes the variable acceleration within the min/max values with the given amount
  /// </summary>
  /// <param name="p_amount"> 
  /// The Amount to change acceleration 
  /// </param>
  /// <returns>
  /// returns result of change
  /// * 0 = acceleration is at zero
  /// * 1 = acceleration is at max
  /// * -1 = acceleration is negative
  /// * 2 = acceleration is increasing
  /// * -2 = acceleration is decreasing
  /// </returns>
  public int ChangeAcceleration(float amount)
  {
    m_acceleration += m_acceleration < m_maxAcceleration && amount > 0 ? amount : 
                      m_acceleration > m_maxDeacceleration && amount < 0 ? amount : 0;

    m_acceleration = (int)m_acceleration;
    
    int result = m_acceleration == 0 ? 0 :
                 m_acceleration == m_maxAcceleration ? 1 :
                 m_acceleration == m_maxDeacceleration ? -1 :
                 m_acceleration > 0 ? 2 : -2;

    Debug.Log("Result of input: " + result);

    return result;
  }

  void Update()
  {
    ChangeAcceleration(m_Gamepad.rightTrigger.ReadValue());
    ChangeAcceleration(m_Gamepad.leftTrigger.ReadValue() * -1);

    transform.Rotate(Vector3.back * m_Gamepad.leftStick.ReadValue().x);

    m_rigidBody.AddRelativeForce(Vector3.up * m_acceleration * Time.deltaTime);
  }
}
