document.addEventListener("DOMContentLoaded", () => {
    const form = document.querySelector("form");
    if (!form) return;

    form.addEventListener("submit", async (e) => {
        e.preventDefault();
        const formData = new FormData(form);

        try {
            const res = await fetch("/submit", { method: "POST", body: formData });

            if (res.redirected) {
                window.location.href = res.url;
            } else if (res.ok) {
                window.location.href = "/confirmation/index.html";
            } else {
                const msg = await res.text();
                alert(msg || "Submission failed.");
            }
        } catch (err) {
            console.error(err);
            alert("Network error submitting form.");
        }
    });
});
