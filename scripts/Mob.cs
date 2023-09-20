using Godot;

namespace Creeps2D.scripts;

public partial class Mob : RigidBody2D
{
    public override void _Ready()
    {
        var animatedSprint2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        string[] mobTypes = animatedSprint2D.SpriteFrames.GetAnimationNames();
        animatedSprint2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
    }

    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
        QueueFree();
    }
}