import { test, expect } from '@playwright/test';

test('Full-stack health check and UI load', async ({ page, request }) => {
    const apiResponse = await request.get('http://localhost:5209/swagger/v1/swagger.json');
    expect(apiResponse.status()).toBe(200);

    await page.goto('/');
    await expect(page.locator('#root')).toBeVisible();
});