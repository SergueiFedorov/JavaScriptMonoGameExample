(function () {

    sceneOne = new scene();

    for (var count = 0; count < 30; count++) {
        var myObject = new sprite(count * 30, count * count, 0);
        sceneOne.addSprite(myObject);

        myObject.scale = 0.1;
        myObject.setTexture("Sprite_logo");

        myObject.update = function (gametime) {
            this.rotation += 0.001 * gametime;
        }
    }

    sceneOne.update = function (gametime) {

    }

    setCurrentScene(sceneOne);

}());
