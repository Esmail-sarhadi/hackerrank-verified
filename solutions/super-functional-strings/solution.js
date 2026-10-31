// Super Functional Strings
// https://www.hackerrank.com/challenges/super-functional-strings
//
// Sum, over every distinct substring, of len^d where d is the number of
// distinct characters in it, modulo 1e9+7.
//
// Suffix array + Kasai LCP: each suffix contributes the lengths from
// lcp[i]+1 to n-sa[i], and the distinct-character count is constant between
// consecutive "next occurrence" offsets, so those ranges are summed with
// precomputed prefix sums of L^d.
//
// Status: verified against a brute force that enumerates every distinct
// substring and computes the power directly.

function superFunctionalStrings(s) {
    const MOD = 1000000007;

    s = (s || '').trim();
    const n = s.length;
    if (n === 0) return 0;

    // ---------- suffix array ----------
    const sa = new Int32Array(n);
    const rk = new Int32Array(n);
    const tmp = new Int32Array(n);
    const nr = new Int32Array(n);
    const cnt = new Int32Array(Math.max(n + 2, 27));

    let maxv = 0;
    for (let i = 0; i < n; i++) {
        let c = s.charCodeAt(i) - 97;
        if (c < 0 || c > 25) c = 0;
        rk[i] = c;
        if (c > maxv) maxv = c;
    }
    for (let i = 0; i <= maxv; i++) cnt[i] = 0;
    for (let i = 0; i < n; i++) cnt[rk[i]]++;
    for (let i = 1; i <= maxv; i++) cnt[i] += cnt[i - 1];
    for (let i = n - 1; i >= 0; i--) sa[--cnt[rk[i]]] = i;

    for (let k = 1; k < 2 * n + 2; k <<= 1) {
        let p = 0;
        for (let i = n - k; i < n; i++) if (i >= 0) tmp[p++] = i;
        for (let i = 0; i < n; i++) if (sa[i] >= k) tmp[p++] = sa[i] - k;

        const classes = rk[sa[n - 1]] + 1;
        for (let i = 0; i < classes; i++) cnt[i] = 0;
        for (let i = 0; i < n; i++) cnt[rk[i]]++;
        for (let i = 1; i < classes; i++) cnt[i] += cnt[i - 1];
        for (let i = n - 1; i >= 0; i--) sa[--cnt[rk[tmp[i]]]] = tmp[i];

        nr[sa[0]] = 0;
        for (let i = 1; i < n; i++) {
            const a = sa[i - 1], b = sa[i];
            const s1 = a + k < n ? rk[a + k] : -1;
            const s2 = b + k < n ? rk[b + k] : -1;
            nr[b] = nr[a] + (rk[a] !== rk[b] || s1 !== s2 ? 1 : 0);
        }
        rk.set(nr);
        if (rk[sa[n - 1]] === n - 1) break;
    }

    // ---------- LCP (Kasai) ----------
    const pos = new Int32Array(n);
    for (let i = 0; i < n; i++) pos[sa[i]] = i;
    const lcp = new Int32Array(n);
    let h = 0;
    for (let i = 0; i < n; i++) {
        const p = pos[i];
        if (p === 0) { h = 0; continue; }
        const j = sa[p - 1];
        while (i + h < n && j + h < n && s[i + h] === s[j + h]) h++;
        lcp[p] = h;
        if (h) h--;
    }

    // ---------- next occurrence of each letter ----------
    const nextOcc = new Int32Array((n + 1) * 26).fill(-1);
    for (let i = n - 1; i >= 0; i--) {
        const b = i * 26, nb = (i + 1) * 26;
        for (let c = 0; c < 26; c++) nextOcc[b + c] = nextOcc[nb + c];
        let c0 = s.charCodeAt(i) - 97;
        if (c0 < 0 || c0 > 25) c0 = 0;
        nextOcc[b + c0] = i;
    }

    // ---------- prefix sums of L^d ----------
    const sumPow = new Array(27);
    const cur = new Float64Array(n + 1);
    for (let L = 1; L <= n; L++) cur[L] = L;
    for (let d = 1; d <= 26; d++) {
        const arr = new Int32Array(n + 1);
        let acc = 0;
        for (let L = 1; L <= n; L++) {
            acc += cur[L];
            if (acc >= MOD) acc -= MOD;
            arr[L] = acc;
        }
        sumPow[d] = arr;
        for (let L = 1; L <= n; L++) cur[L] = (cur[L] * L) % MOD;
    }

    // ---------- sum over distinct substrings ----------
    const offs = new Int32Array(26);
    let ans = 0;

    for (let i = 0; i < n; i++) {
        const p = sa[i];
        const maxL = n - p;
        const start = lcp[i] + 1;
        if (start > maxL) continue;

        let r = 0;
        for (let c = 0; c < 26; c++) {
            const q = nextOcc[p * 26 + c];
            if (q !== -1) offs[r++] = q - p;
        }
        for (let a = 1; a < r; a++) {
            const v = offs[a];
            let b = a - 1;
            while (b >= 0 && offs[b] > v) { offs[b + 1] = offs[b]; b--; }
            offs[b + 1] = v;
        }

        let left = 1;
        for (let k = 0; k < r && left <= maxL; k++) {
            const right = k + 1 < r ? offs[k + 1] : maxL;
            const lo = left > start ? left : start;
            const hi = right < maxL ? right : maxL;
            if (lo <= hi) {
                const sp = sumPow[k + 1];
                ans += sp[hi] - sp[lo - 1];
                ans %= MOD;
                if (ans < 0) ans += MOD;
            }
            left = right + 1;
        }
    }

    return ans % MOD;
}

module.exports = { superFunctionalStrings };
