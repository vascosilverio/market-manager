document.addEventListener('DOMContentLoaded', function () {
    const xCoordInput = document.getElementById('xCoord');
    const yCoordInput = document.getElementById('yCoord');
    const intersectionPoints = document.querySelectorAll('.intersection-point');

    intersectionPoints.forEach(point => {
        point.addEventListener('click', function () {
            const x = this.getAttribute('data-x');
            const y = this.getAttribute('data-y');
            updateCoordinates(x, y);
            highlightPoint(this);
        });
    });

    function updateCoordinates(x, y) {
        xCoordInput.value = x;
        yCoordInput.value = y;

        // Send the coordinates to the server
        fetch('/Bancas/UpdateCoordinates', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ x: parseInt(x), y: parseInt(y) }),
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    console.log('Coordinates updated successfully');
                    // You can add more logic here, like updating the Bancas list
                }
            })
            .catch(error => console.error('Error:', error));
    }

    function highlightPoint(point) {
        // Remove highlight from all points
        intersectionPoints.forEach(p => p.setAttribute('fill', 'transparent'));
        // Highlight the clicked point
        point.setAttribute('fill', 'red');
    }
});