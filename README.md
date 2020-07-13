# pyramid_test
Game made as part of Pyramid Games recruitment process.

Project made with Unity 2018.2.21f1.

The goal of the game is to score as many points as you can. There is a ball on the screen and a flag with a hole at random position (in the X plane) located on the right side of the screen and a point counter. If you manage to get the ball into the hole you get a point, if it falls on the ground your highscore is saved, you lose all your current points and can start over.

As soon as you press and hold a button (default spacebar), a parabola displaying the movement of the ball begins to show and its range increases. When the button is released or maximum range is reached, the ball is thrown in accordance with the parabola. If the ball falls in the hole a point is added, another level is generated with a differently placed flag and faster parabola growth rate. If the ball did not hit the hole, game over panel appears with the current score, the best score so far (stored in a file) and a button to restart the game.