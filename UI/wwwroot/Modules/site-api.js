/**
 * ApiClient.js
 * Generic fetch-based client for calling MVC controller endpoints
 * that return Json(ApiResponse<T>) — i.e. { success, error, data }.
 *
 * Matches BL.Common.ApiResponse<T> as used in GenericApiClient / MvcAuthService.
 */
const ApiClient = (function () {

    // NOTE: ASP.NET Core's default Json() serializer (System.Text.Json)
    // camelCases property names by default: Success -> success, Error -> error, Data -> data.
    // If your ApiResponse<T> is serialized differently (e.g. Newtonsoft with PascalCase,
    // or [JsonPropertyName] overrides), adjust these keys to match exactly.
    const KEYS = {
        success: 'success',
        error: 'error',
        data: 'data'
    };

    function getAntiForgeryToken() {
        const input = document.querySelector('input[name="__RequestVerificationToken"]');
        return input ? input.value : null;
    }

    function showLoader(show) {
        const loader = document.getElementById('loader');
        if (loader) loader.style.display = show ? 'flex' : 'none';
    }

    function notifyError(message) {
        if (typeof toastr !== 'undefined') {
            toastr.error(message || 'Something went wrong.');
        } else {
            console.error(message);
            alert(message || 'Something went wrong.');
        }
    }

    async function request({
        url,
        method = 'GET',
        data = null,
        showLoading = true,
        redirectOn401 = true
    }) {
        const headers = {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest'
        };

        const token = getAntiForgeryToken();
        if (token) headers['RequestVerificationToken'] = token;

        const fetchOptions = { method, headers, credentials: 'same-origin' };

        if (data !== null && (method === 'POST' || method === 'PUT')) {
            fetchOptions.body = JSON.stringify(data);
        }

        if (showLoading) showLoader(true);

        try {
            const res = await fetch(url, fetchOptions);

            if (res.status === 401 && redirectOn401) {
                window.location.href = '/Account/Login';
                return { success: false, error: 'Unauthorized', data: null };
            }

            let body = null;
            try {
                body = await res.json();
            } catch {
                // No JSON body (e.g. empty 204 response)
            }

            if (!res.ok && !body) {
                const errMsg = `Request failed with status ${res.status}`;
                notifyError(errMsg);
                return { success: false, error: errMsg, data: null };
            }

            const success = body?.[KEYS.success] ?? res.ok;
            const error = body?.[KEYS.error];
            const responseData = body?.[KEYS.data];

            if (!success && error) {
                notifyError(error);
            }

            return { success, error, data: responseData, raw: body };
        } catch (ex) {
            const errMsg = 'Network error. Please check your connection.';
            notifyError(errMsg);
            return { success: false, error: errMsg, data: null };
        } finally {
            if (showLoading) showLoader(false);
        }
    }

    return {
        get: (url, options = {}) => request({ url, method: 'GET', ...options }),
        post: (url, data, options = {}) => request({ url, method: 'POST', data, ...options }),
        put: (url, data, options = {}) => request({ url, method: 'PUT', data, ...options }),
        delete: (url, options = {}) => request({ url, method: 'DELETE', ...options }),
        request // escape hatch for custom cases
    };
})();