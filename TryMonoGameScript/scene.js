
for (var count = 0; count < 5; count++) {
    var newSprite = new sprite(count * 100, count * 100, 0);
    newSprite.setTexture("Sprite_logo");

    newSprite.update = function (gameTime) {
        this.rotation += 0.001 * gameTime;
        this.x += 0.1 * gameTime;
    }

    currentScene.addSprite(newSprite);
}
