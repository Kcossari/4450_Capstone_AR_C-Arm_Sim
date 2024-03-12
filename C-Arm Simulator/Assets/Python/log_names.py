import UnityEngine as ue

objects = ue.Object.FindObjectsOfType(ue.GameObject)
for go in objects:
    print(go.name)
