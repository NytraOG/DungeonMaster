using System;
using UnityEngine;

namespace Entities
{
    public class Base : MonoBehaviour
    {
        public Base() => Id = Guid.NewGuid();

        public Guid Id { get; set; }
    }
}