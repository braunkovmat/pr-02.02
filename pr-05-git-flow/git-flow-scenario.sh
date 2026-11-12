#!/bin/bash
# ПР №5, вариант 3: сценарий Git Flow для АИС «Интеграционный шлюз интернет-магазина».
# Запуск (Git Bash): bash git-flow-scenario.sh
set -e
rm -rf shop-gateway && mkdir shop-gateway && cd shop-gateway

echo "== Репозиторий и ветка develop =="
git init -b main
git commit -qm "Initial commit" --allow-empty
git tag v1.0.0
git checkout -b develop

echo "== Группа 1: платёжный модуль =="
git checkout -b feature/payment-gateway develop
echo "class PaymentService {}" > Payment.cs && git add . && git commit -qm "feat(payment): add PaymentService"
echo "== Группа 2: склад =="
git checkout -b feature/warehouse-sync develop
echo "class StockSync {}" > Stock.cs && git add . && git commit -qm "feat(warehouse): add stock synchronization"
echo "== Группа 3: каталог =="
git checkout -b feature/catalog-import develop
echo "class CatalogImport {}" > Catalog.cs && git add . && git commit -qm "feat(catalog): add catalog import"

echo "== Слияние feature в develop =="
git checkout develop
git merge --no-ff feature/payment-gateway   -m "Merge feature/payment-gateway into develop"
git merge --no-ff feature/warehouse-sync    -m "Merge feature/warehouse-sync into develop"
git merge --no-ff feature/catalog-import    -m "Merge feature/catalog-import into develop"

echo "== Релиз =="
git checkout -b release/v1.1 develop
echo "1.1.0" > VERSION && git add . && git commit -qm "chore(release): bump version to 1.1.0"
git checkout main
git merge --no-ff release/v1.1 -m "Merge release/v1.1 into main"
git tag -a v1.1.0 -m "Release 1.1.0"
git checkout develop
git merge --no-ff release/v1.1 -m "Merge release/v1.1 back into develop"
git branch -d release/v1.1

echo "== Hotfix =="
git checkout -b hotfix/payment-timeout main
echo "timeout=30" > Hotfix.txt && git add . && git commit -qm "fix(payment): increase gateway timeout"
git checkout main
git merge --no-ff hotfix/payment-timeout -m "Merge hotfix/payment-timeout into main"
git tag -a v1.1.1 -m "Hotfix 1.1.1"
git checkout develop
git merge --no-ff hotfix/payment-timeout -m "Merge hotfix/payment-timeout into develop"
git branch -d hotfix/payment-timeout feature/payment-gateway feature/warehouse-sync feature/catalog-import

echo "== Итог =="
git log --graph --oneline --decorate --all
git branch
git tag
