﻿document.addEventListener('DOMContentLoaded', () => {
    console.log("JavaScript loaded successfully");

    const previewSize = 150

    document.querySelectorAll("[data-modal][data-project-id]").forEach(function (icon) {
        icon.addEventListener("click", function () {
            const projectId = this.getAttribute("data-project-id");

            fetch(`/Projects/Edit?id=${projectId}`)
                .then(response => response.text())
                .then(html => {

                    document.getElementById("editProjectModalContainer").innerHTML = '';
                    document.getElementById("editProjectModalContainer").innerHTML = html;
                    document.querySelectorAll('.modal-custom').forEach(modal => {
                        modal.style.display = 'none';
                    });

                    // Visa modalen direkt efter att innehållet laddats
                    const modal = document.querySelector('#editProjectModalContainer .modal-custom');
                    if (modal) {
                        modal.style.display = 'flex';
                    }
                });
        });
    });

    // open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]')
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target')
            const modal = document.querySelector(modalTarget)

            if (modal)
                modal.style.display = 'flex';
        })
    })

    // close modal
    const closeButtons = document.querySelectorAll('[data-close="true"]')
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal-custom')
            if (modal)
                modal.style.display = 'none';

            //clear formdata
            modal.querySelectorAll('form').forEach(form => {
                form.reset()

                const imagePreview = form.querySelector('.image-preview')
                if (imagePreview)
                    imagePreview.src = ''

                const imagePreviewer = form.querySelector('.image-previewer')
                if (imagePreviewer)
                    imagePreviewer.classList.remove('selected')
            })

        })
    })

    // handle image-previewer
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type="file"]')
        const imagePreview = previewer.querySelector('.image-preview')

        previewer.addEventListener('click', () => fileInput.click())

        fileInput.addEventListener('change', ({ target: { files } }) => {
            const file = files[0]
            if (file)
                processImage(file, imagePreview, previewer, previewSize)
        })
    })

    // handle submit forms
    const forms = document.querySelectorAll('form')
    forms.forEach(form => {
        console.log("Form submitted via JS");
        form.addEventListener('submit', async (e) => {


            //Om man inte har e.preventDefault här så loggas man inte in av nån anledning
            //e.preventDefault()


            clearErrorMessages(form)


            const formData = new FormData(form)



            try {
                console.log("Form action URL:", form.action);
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                })

                if (res.ok) {
                    const modal = form.closest('.modal-custom')
                    if (modal)
                        modal.style.display = 'none';

                    window.location.reload();
                }
                else if (res.status === 400) {
                    const data = await res.json()


                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            let input = form.querySelector(`[name="${key}"]`)
                            if (input) {
                                input.classList.add('input-validation-error')
                            }

                            let span = form.querySelector(`[data-valmsg-for="${key}"]`)
                            if (span) {
                                span.innerText = data.errors[key].join('\n');
                                span.classList.add('field-validation-error')
                                //span.classList.remove('field-validation-valid')
                            }
                        })
                    }
                }

            }
            catch {
                console.log('error submitting the form')
            }
        })
    })
})

function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error')
    })
    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = ''
        span.classList.remove('field-validation-error')
    })
}

function addErrorMessage(key, errorMessage) {

}



async function loadImage(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader()

        reader.onerror = () => reject(new Error("Failed to load file."))
        reader.onload = (e) => {
            const img = new Image()
            img.onerror = () => reject(new Error("Failed to load image"))
            img.onload = () => resolve(img)
            img.src = e.target.result
        }
        reader.readAsDataURL(file)
    })
}

async function processImage(file, imagePreview, previewer, previewSize = 150) {
    try {
        const img = await loadImage(file)
        const canvas = document.createElement('canvas')
        canvas.width = previewSize
        canvas.height = previewSize

        const ctx = canvas.getContext('2d')
        ctx.drawImage(img, 0, 0, previewSize, previewSize)
        imagePreview.src = canvas.toDataURL('image/jpeg')
        previewer.classList.add('selected')
    }
    catch (error) {
        console.error('Failed on image-processing:', error)
    }
}