using System;
using Godot;

namespace Creeps2D.scripts;

public partial class Player : Area2D
{
    [Signal]
    public delegate void HitEventHandler();
    
    [Export] public int Speed { get; set; } = 400;

    public Vector2 screenSize;

    public override void _Ready()
    {
        screenSize = GetViewportRect().Size;
        
        Hide();
    }

    public override void _Process(double delta)
    {
        var velocity = Vector2.Zero;
        
        if (Input.IsActionPressed("move_right"))
        {
            velocity.X += 1;
        }

        if (Input.IsActionPressed("move_left"))
        {
            velocity.X -= 1;
        }

        if (Input.IsActionPressed("move_down"))
        {
            velocity.Y += 1;
        }

        if (Input.IsActionPressed("move_up"))
        {
            velocity.Y -= 1;
        }

        var animatedSprite2d = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            animatedSprite2d.Play();
        }
        else
        {
            animatedSprite2d.Stop();
        }

        Position += velocity * (float) delta;
        Position = new Vector2(
            x: Mathf.Clamp(Position.X, 0, screenSize.X),
            y: Mathf.Clamp(Position.Y, 0, screenSize.Y)
        );

        if (velocity.X != 0)
        {
            animatedSprite2d.Animation = "walk";
            animatedSprite2d.FlipV = false;
            animatedSprite2d.FlipH = velocity.X < 0;
        } else if (velocity.Y != 0)
        {
            animatedSprite2d.Animation = "up";
            animatedSprite2d.FlipV = velocity.Y > 0;
        }
    }

    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
    }
    
    private void OnBodyEntered(PhysicsBody2D body)
    {
        Hide();
        EmitSignal(SignalName.Hit);
        GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }
}