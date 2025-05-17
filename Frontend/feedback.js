function createDialog(meta) {
    // Создание элемента dialog
    const dialog = document.createElement("dialog");
    dialog.setAttribute("id", "dynamicDialog");
    dialog.innerHTML = `
        <p>Оставить отзыв:</p>
        <textarea id="reviewText" rows="4" cols="30" placeholder="Введите ваш отзыв..."></textarea>
        <br>
        <button id="closeDialog">Закрыть</button>
        <button id="apply">Отправить</button>
    `;
    

    document.body.appendChild(dialog);
    dialog.showModal();
    
    const close = () => {
        dialog.close();
        dialog.remove();
    }

    document.getElementById("closeDialog").addEventListener("click", close);
    document.getElementById("apply").addEventListener("click", () => {
        const reviewText = dialog.querySelector('textarea').value;

        // Парсим строку JSON в объект
        const parsedMeta = JSON.parse(meta);

        // Достаем значение id из объекта user
        const userId = parsedMeta.user.id;


        // Отправка HTTP POST-запроса
        fetch('http://4810897-ta77728.twc1.net:8080/Feedback/PostFeedback', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                'message': reviewText,
                'clientId': userId
            }),
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok ' + response.statusText);
                }
            })
            .then(data => {
                console.log('Успешно отправлено:', data);
                close();
            })
            .catch(error => {
                console.error('Ошибка при отправке отзыва:', error);
                alert('Произошла ошибка при отправке отзыва.'); // Уведомление об ошибке
            });
    });
}

const openFeedbackDialog = (evt) => {
    const meta = evt.target.dataset.feedbackMeta

    if (meta) {
        createDialog(meta)
    }
}

window.__feedbackAPI = {
    openFeedbackDialog: createDialog,
}

document.addEventListener('DOMContentLoaded', () => {
    const triggers = document.querySelectorAll('[data-feedback-trigger]')
    triggers.forEach((trigger) => {
        trigger.addEventListener('click', openFeedbackDialog)
    })
}, { once: true })
